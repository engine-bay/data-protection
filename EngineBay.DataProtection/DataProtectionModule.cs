namespace EngineBay.DataProtection
{
    using EngineBay.Core;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class DataProtectionModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            // register data protection key store for Personally Identifiable Information Encryption
            DataProtectionKeyStoreConfiguration.AddKeyStoreProvider(services);
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }

        public WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }
    }
}