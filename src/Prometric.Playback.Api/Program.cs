using Convey;
using Convey.Types;
using Convey.WebApi.CQRS;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Convey.Logging;
using Prometric.Playback.Application;
using Prometric.Playback.Infrastructure;
using System.Text;
using Prometric.Playback.Application.Commands;
using Prometric.Playback.Application.Queries;
using Prometric.Playback.Application.DTO;
using Prometric.Playback.Infrastructure.Scopes;
using Convey.Persistence.Redis;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace Prometric.Playback.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
// ConfigureServices is where dotnet Core configures its Dependency Injection engine.
.ConfigureServices(services => services
    .AddConvey() // => Mandatory extension method. Base configuration for Configure Convey
    .AddWebApi() // => Mandatory: All related WebApi registrations and configurations
    .AddApplication() // => Each project will have a grouping extension method: Application
    .AddInfrastructure() // => Each project will have a grouping extension method: Infrastructure
.AddRedis()
    .Build())
// Configure is where we setup the 'actions' that occur in every single request.
.Configure(app => app
    .UseInfrastructure()
    .UseDispatcherEndpoints(endpoints => endpoints
        .Get(Routes.Default, httpContext =>
        {
            var appOptions = httpContext.RequestServices.GetService<AppOptions>();
            return httpContext.Response.WriteAsync($"{appOptions.Name} {appOptions.Version}");
        })
       // Recordings
       .Post<AddRecording>(Routes.Recordings,
            afterDispatch: (cmd, ctx) => ctx.Response.Created($"{Routes.Recordings}"))
        .Get<GetRecording, RecordingDto>($"{Routes.Recordings}/{{recordingId}}")
       // Compositions
       .Post<AddComposition>(Routes.Composition,
            afterDispatch: (cmd, ctx) => ctx.Response.Created($"{Routes.Composition}/{cmd.CompositionId}"))
        .Get<GetComposition, CompositionDto>($"{Routes.Composition}/{{compositionId}}")
        // Health
        .Get(Routes.Health, httpContext =>
        {
            return httpContext.Response.WriteAsync("OK");
        })

        .Post("webhook", 
        async (ctx) => {
            
            ctx.Request.EnableBuffering(); // Enable seeking            
            var bodyAsText = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
            
            //  Set the position of the stream to 0 to enable rereading
            ctx.Request.Body.Position = 0;            
            string[] args = bodyAsText.Split("&");
            var dict = new Dictionary<string, string>();
            foreach (string arg in args) {
                var kv = arg.Split("=");
                dict.Add(kv[0], kv[1]);
            }

            var webhook = new Webhook();
            webhook.RoomStatus = dict[nameof(webhook.RoomStatus)];
            webhook.RoomType = dict[nameof(webhook.RoomType)];
            webhook.RoomSid = dict[nameof(webhook.RoomSid)];
            webhook.RoomName = dict[nameof(webhook.RoomName)];
            webhook.RoomDuration = dict[nameof(webhook.RoomDuration)];
            webhook.SequenceNumber = dict[nameof(webhook.SequenceNumber)];
            webhook.StatusCallbackEvent = dict[nameof(webhook.StatusCallbackEvent)];
            webhook.Timestamp = dict[nameof(webhook.Timestamp)];
            webhook.AccountSid = dict[nameof(webhook.AccountSid)];

            await ctx.SendAsync(webhook);
        })
    )
)
.UseLogging();
        }
    }
}
