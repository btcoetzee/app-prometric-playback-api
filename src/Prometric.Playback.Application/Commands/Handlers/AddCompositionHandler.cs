using Convey.CQRS.Commands;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;

using Prometric.Playback.Application.Exceptions;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class AddCompositionHandler : ICommandHandler<AddComposition>
    {
        private const string BASE_COMPOSITION_PATH = "/app/twilio-compositions";
        private readonly ILogger<AddCompositionHandler> _log;
        private WebClient _webClient;
        private IConfigurationSection _ams { get; set; }
        private const string AdaptiveStreamingTransformName = "DefaultProProctorMediaTransform";
        private AddComposition _commandIn { get; set; }
        private string _compositionFileName { get; set; }
        private string _compositionFilePath { get; set; }
        public AddCompositionHandler(ILogger<AddCompositionHandler> log, IConfiguration config) { 
            // Save AMS creds
            _ams = config.GetSection("ams");
            // Setup web client, provide twilio accountId/authToken for basic auth req
            var twilio = config.GetSection("twilio");
            _webClient = new WebClient();
            _webClient.Credentials = new NetworkCredential(twilio["AccountSID"], twilio["AuthToken"]);
            _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler (PushToAMSAsync);
            // Logger
            _log = log;
        }

        public async Task HandleAsync(AddComposition command)
        {
            // Only proceed on completed composition events
            if (command.StatusCallbackEvent != "composition-available")
                return;
            // Save the command object for later async work
            _commandIn = command;
            // Download the composition to disc named by the exam session label
            _compositionFileName = _commandIn.ExamSessionLabel + ".mp4";
            _compositionFilePath = Path.Combine(BASE_COMPOSITION_PATH, _compositionFileName);
            var uri = CreateCompositionUri(command.CompositionUri);
            _log?.LogInformation($"Downloading Twilio composition file - from {uri}, to {_compositionFilePath}");
            _webClient.DownloadFileAsync(uri, _compositionFilePath);
        }

        public static Uri CreateCompositionUri(string compositionUri)
        {
            return new Uri(
                "https://video.twilio.com" + 
                WebUtility.UrlDecode(compositionUri) + 
                "/Media"
            );
        }

        // Async Event handler that pushes transcode job to AMS
        public async void PushToAMSAsync(object sender, EventArgs e) {
            Console.WriteLine("Download Completed... Uploading to AMS");
            _log?.LogInformation($"Twilio Composition download complete to {_compositionFilePath}");
            string jobName = _commandIn.ExamSessionLabel;
            string amsAssetNameIn = "assetIn--" + _compositionFileName;
            string amsAssetNameOut = _commandIn.ExamSessionLabel;

            IAzureMediaServicesClient client = await CreateMediaServicesClientAsync(_ams);

            // Ensure we have the desired encoding Transform. This is really a one time setup operation.
            _ = await GetOrCreateTransformAsync(
                client,
                _ams["ResourceGroup"],
                _ams["AccountName"],
                AdaptiveStreamingTransformName);

            // Create a new input Asset and upload the specified local video file into it.
            _ = await CreateInputAssetAsync(
                client,
                _ams["ResourceGroup"],
                _ams["AccountName"],
                amsAssetNameIn,
                _compositionFilePath);

            // Use the name of the created input asset to create the job input.
            _ = new JobInputAsset(assetName: amsAssetNameIn);

            // Output from the encoding Job must be written to an Asset
            Asset outputAsset = await CreateOutputAssetAsync(
                client,
                _ams["ResourceGroup"],
                _ams["AccountName"],
                amsAssetNameOut,
                _compositionFileName);

            // Submit job to AMS
            _ = await SubmitJobAsync(
                client,
                _ams["ResourceGroup"],
                _ams["AccountName"],
                AdaptiveStreamingTransformName,
                jobName,
                amsAssetNameIn,
                outputAsset.Name,
                _log);

            // Remove local files
            Cleanup(_compositionFileName);
            Console.WriteLine("Job Submitted.");
        }

        // Get a token with a service principal from configuration
        private static async Task<ServiceClientCredentials> GetCredentialsAsync(IConfigurationSection config)
        {
            ClientCredential clientCredential = new ClientCredential(config["AadClientId"], config["AadSecret"]);
            return await ApplicationTokenProvider.LoginSilentAsync(
                config["AadTenantId"],
                clientCredential,
                ActiveDirectoryServiceSettings.Azure);
        }

        // Create AMS client with credentials
        private static async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(IConfigurationSection config)
        {
            var credentials = await GetCredentialsAsync(config);
            return new AzureMediaServicesClient(credentials){ SubscriptionId = config["SubscriptionId"] };
        }

        // Get the transform or create it. This should be a AMS deployment detail in the future
        private static async Task<Transform> GetOrCreateTransformAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName)
        {
            Transform transform = await client.Transforms.GetAsync(resourceGroupName, accountName, transformName);
            if (transform == null)
            {
                // Create the Transform 
                TransformOutput[] output = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output);
            }
            return transform;
        }

        // Creates a new input Asset and uploads the specified local video file into it.
        private static async Task<Asset> CreateInputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string assetName,
            string fileToUpload)
        {
            // Call Media Services API to create an Asset.
            // This method creates a container in storage for the Asset.
            // The files (blobs) associated with the asset will be stored in this container.
            Asset asset = await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset());

            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the exparation time for the SAS URL.
            var response = await client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            CloudBlobContainer container = new CloudBlobContainer(sasUri);
            var blob = container.GetBlockBlobReference(Path.GetFileName(fileToUpload));

            // Use Strorage API to upload the file into the container in storage.
            await blob.UploadFromFileAsync(fileToUpload);

            return asset;
        }

        // Create the ouput asset. The output from the encoding Job must be written to an Asset.
        private static async Task<Asset> CreateOutputAssetAsync(IAzureMediaServicesClient client,
            string resourceGroupName, string accountName, string assetName,
            string localFileName)
        {
            // Check if an Asset already exists, cleanup and throw if so. 
            // This should prevent dupes from being processed. Unlikely to happen with unique file names
            Asset existingOutputAsset = await client.Assets.GetAsync(resourceGroupName, accountName, assetName);
            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (existingOutputAsset != null)
            {
                Cleanup(localFileName);
                throw new AMSOutputAssetAlreadyExistsException(assetName);
            }
            // Create the output asset
            Asset outputAsset = await client.Assets.CreateOrUpdateAsync(
                resourceGroupName,
                accountName,
                outputAssetName,
                asset);
            return outputAsset;
        }

        // Pushes a job to AMS with specified input/output assets and transform
        private static async Task<Job> SubmitJobAsync(IAzureMediaServicesClient client,
            string resourceGroupName, string accountName, string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName,
            ILogger log)
        {
            // Set job input/output
            JobInput jobInput = new JobInputAsset(assetName: inputAssetName);
            JobOutput[] jobOutputs = { new JobOutputAsset(outputAssetName) };

            // Create the job
            log?.LogInformation($"Creating AMS Job {jobName}");
            Job job = await client.Jobs.CreateAsync(
                resourceGroupName,
                accountName,
                transformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs,
                });
            log?.LogInformation($"AMS Job created {jobName}");
            return job;
        }

        private static void Cleanup(string compositionFileName)
        {
            Console.WriteLine("Performing cleanup...");
            Console.WriteLine("Remove local files...");
            File.Delete(Path.Combine(BASE_COMPOSITION_PATH, compositionFileName));
            Console.WriteLine("Cleanup Complete.");
        }
    }
}
