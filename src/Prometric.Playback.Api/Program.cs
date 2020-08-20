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
using Convey.Persistence.Redis;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System;

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
        // Health
        .Get(Routes.Health, httpContext =>
        {
            return httpContext.Response.WriteAsync("OK");
        })
        // Recording-complete webhook callback
        .Post(Routes.Recordings, 
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
            // Only forward the request if the event is a recording-completed event and the participant's identity is the candidate
            if (dict["StatusCallbackEvent"] == "recording-completed" 
                && dict["ParticipantIdentity"] == "candidate"
                && dict["Container"] == "mkv") {
                var addRecording = new AddRecording();
                addRecording.RoomSid = dict[nameof(addRecording.RoomSid)];
                addRecording.RoomName = dict[nameof(addRecording.RoomName)];
                addRecording.StatusCallbackEvent = dict[nameof(addRecording.StatusCallbackEvent)];
                addRecording.Timestamp = System.DateTime.Parse(HttpUtility.UrlDecode(dict[nameof(addRecording.Timestamp)]));
                addRecording.ParticipantSid = dict[nameof(addRecording.ParticipantSid)];
                addRecording.ParticipantIdentity = dict[nameof(addRecording.ParticipantIdentity)];

                Console.WriteLine(bodyAsText);
                await ctx.SendAsync(addRecording);
            }
        })
        .Post(Routes.Composition,
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

            /**
            * KYLE REPLACE ME
            **/
            // Only forward the request if the event is a recording-completed event and the participant's identity is the candidate
            if (dict["StatusCallbackEvent"] == "recording-completed" 
                && dict["ParticipantIdentity"] == "candidate"
                && dict["Container"] == "mkv") {
                var addRecording = new AddRecording();
                addRecording.RoomSid = dict[nameof(addRecording.RoomSid)];
                addRecording.RoomName = dict[nameof(addRecording.RoomName)];
                addRecording.StatusCallbackEvent = dict[nameof(addRecording.StatusCallbackEvent)];
                addRecording.Timestamp = System.DateTime.Parse(HttpUtility.UrlDecode(dict[nameof(addRecording.Timestamp)]));
                addRecording.ParticipantSid = dict[nameof(addRecording.ParticipantSid)];
                addRecording.ParticipantIdentity = dict[nameof(addRecording.ParticipantIdentity)];

                Console.WriteLine(bodyAsText);
                await ctx.SendAsync(addRecording);
            }
        })
    )
)
.UseLogging();
        }
    }
}
