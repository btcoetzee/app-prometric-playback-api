using Convey;
using Convey.Auth;
using Convey.CQRS.Queries;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.Redis;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometric.Playback.Application;
using Prometric.Playback.Core.Repositories;
using Prometric.Playback.Infrastructure.Logging;
using Prometric.Playback.Infrastructure.Redis.Repositories;
using Prometric.Playback.Infrastructure.Scopes;

namespace Prometric.Playback.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IBookRepository, RedisBookRepository>();
            builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(HelloWorldScopes.READ_BOOKS, 
                    policyBuilder => policyBuilder.Requirements.Add(
                        new HasScopeRequirement(HelloWorldScopes.READ_BOOKS, 
                            "https://prometric.oktapreview.com/oauth2/ausu831293IEbvvD21d5")));
                options.AddPolicy(HelloWorldScopes.WRITE_BOOKS, 
                    policyBuilder => policyBuilder.Requirements.Add(
                        new HasScopeRequirement(HelloWorldScopes.WRITE_BOOKS, 
                            "https://prometric.oktapreview.com/oauth2/ausu831293IEbvvD21d5")));
            });
            
            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddJwt()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHandlersLogging()
                .AddHttpClient()
                .AddMetrics()
                .AddRedis()
                .AddWebApiSwaggerDocs();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            return app.UseSwaggerDocs()
                      .UseAuthentication()
                      .UseConvey()
                      .UsePublicContracts<ContractAttribute>()
                      .UseMetrics();
        }
    }
}
