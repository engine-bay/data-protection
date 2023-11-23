namespace EngineBay.DataProtection.Tests
{
    using EngineBay.DataProtection;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class DataProtectionServiceRegistrationTests
    {
        [Fact]
        public void AddKeyStoreProviderFileSystemShouldSetPersistKeysToFileSystem()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            DataProtectionKeyStoreConfiguration.AddKeyStoreProvider(services);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var dataProtectionProvider = serviceProvider.GetServices<IDataProtectionProvider>();
            Assert.NotNull(dataProtectionProvider);
        }
    }
}