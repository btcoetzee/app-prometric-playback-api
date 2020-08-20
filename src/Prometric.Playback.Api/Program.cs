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
using Newtonsoft.Json;
using System.Linq;
using Prometric.Playback.Application.Commands;

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
        .Get(Routes.Health, httpContext => httpContext.Response.WriteAsync("OK"))
        // Recording-complete webhook callback
        .Post(Routes.Recordings, async (ctx) => await ctx.SendAsync(await FromRequestBody<AddRecording>(ctx)))
        .Post(Routes.Composition, async (ctx) => await ctx.SendAsync(await FromRequestBody<AddComposition>(ctx)))
    ))
    .UseLogging();
        }

        public static async Task<T> FromRequestBody<T>(HttpContext ctx)
        {
            ctx.Request.EnableBuffering(); // Enable seeking            
            var bodyAsText = await new StreamReader(ctx.Request.Body).ReadToEndAsync();
            //  Set the position of the stream to 0 to enable rereading
            ctx.Request.Body.Position = 0;
            var kvp = HttpUtility.ParseQueryString(bodyAsText);
            var json = JsonConvert.SerializeObject(
                kvp.Keys.Cast<string>().ToDictionary(k => k, k => kvp[k]));
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
