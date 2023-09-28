namespace EngineBay.DataProtection
{
    using EngineBay.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class DataProtectionModule : BaseModule
    {
        public override IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            // register data protection key store for Personally Identifiable Information Encryption
            DataProtectionKeyStoreConfiguration.AddKeyStoreProvider(services);
            return services;
        }
    }
}