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

namespace Prometric.Playback.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
                            // ConfigureServices is where dotnet Core configures its Dependency Injection engine.
                            .ConfigureServices(services => services
                                .AddConvey() // => Mandatory extension method. Base configuration for Configure Convey
                                .AddWebApi() // => Mandatory: All related WebApi registrations and configurations
                                .AddApplication() // => Each project will have a grouping extension method: Application
                                .AddInfrastructure() // => Each project will have a grouping extension method: Infrastructure
                                .Build())
                            // Configure is where we setup the 'actions' that occur in every single request.
                            .Configure(app => app
                                .UseInfrastructure()
                                .UseDispatcherEndpoints(endpoints => endpoints
                                    .Get(Routes.Default, httpContext => {
                                        var appOptions = httpContext.RequestServices.GetService<AppOptions>();
                                        return httpContext.Response.WriteAsync($"{appOptions.Name} {appOptions.Version}");
                                    })
                                // Recordings
                                   .Post<AddRecording>(Routes.Recordings,
                                        afterDispatch: (cmd, ctx) => ctx.Response.Created($"{Routes.Recordings}/{cmd.RecordingId}"),
                                        auth: true, policies: PlaybackScopes.WRITE_RECORDING)
                                    .Get<GetRecording, RecordingDto>($"{Routes.Recordings}/{{recordingId}}", auth: true,  policies: PlaybackScopes.READ_RECORDING)
                                // Health
                                    .Get(Routes.Health, httpContext => {
                                        return httpContext.Response.WriteAsync("OK");
                                    })
                                )
                            )
                            .UseLogging();
    }
}
