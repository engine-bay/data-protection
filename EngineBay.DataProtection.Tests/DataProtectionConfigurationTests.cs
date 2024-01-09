namespace EngineBay.DataProtection.Tests
{
    using System;
    using EngineBay.DataProtection;
    using Xunit;

    public class DataProtectionConfigurationTests
    {
        [Fact]
        public void GetKeyLifetimeReturnsDefaultIfEnvironmentVariableNotPresent()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS, null);
            var keyLifetime = DataProtectionKeyStoreConfiguration.GetKeyLifetime();

            Assert.Equal(DefaultDataProtectionConstants.KeyLifetime, keyLifetime);
        }

        [Fact]
        public void GetKeyLifetimeReturnsEnvironmentVariableValueIfPresent()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS, "10");
            var keyLifetime = DataProtectionKeyStoreConfiguration.GetKeyLifetime();

            Assert.Equal(10, keyLifetime);
        }

        [Fact(Skip = "Tests execute in parallel. This causes AddKeyStoreProviderFileSystemShouldSetPersistKeysToFileSystem to fail. Disable until we can introduce a testing layer into config")]
        public void GetKeyLifetimeThrowsExceptionIfEnvironmentVariableValueIsInvalid()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS, "6");

            Assert.Throws<ArgumentException>(() => DataProtectionKeyStoreConfiguration.GetKeyLifetime());
        }

        [Fact]
        public void GetRedisConnectionStringReturnsEnvironmentVariableValueIfPresent()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONREDISCONNECTIONSTRING, "testRedisConnection");
            var connectionString = DataProtectionKeyStoreConfiguration.GetRedisConnectionString();

            Assert.Equal("testRedisConnection", connectionString);
        }

        [Fact]
        public void GetDataProtectionNamespaceEnvVarSetReturnsValue()
        {
            var testNamespace = "TestNamespace";
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONNAMESPACE, testNamespace);

            var result = DataProtectionKeyStoreConfiguration.GetDataProtectionNamespace();

            Assert.Equal(testNamespace, result);
        }

        [Fact]
        public void GetDataProtectionNamespaceEnvVarNotSetReturnsDefault()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONNAMESPACE, null);

            var result = DataProtectionKeyStoreConfiguration.GetDataProtectionNamespace();

            Assert.Equal(DefaultDataProtectionConstants.Namespace, result);
        }

        [Fact]
        public void GetDataProtectionKeyStoreProviderReturnsCorrectKeyStoreType()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER, DataProtectionKeyStoreTypes.FileSystem.ToString());
            var keyStoreType = DataProtectionKeyStoreConfiguration.GetDataProtectionKeyStoreProvider();
            Assert.Equal(DataProtectionKeyStoreTypes.FileSystem, keyStoreType);
        }

        [Fact]
        public void GetDataProtectionKeyStoreProviderThrowsExceptionWithInvalidConfiguration()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER, "invalid");
            Assert.Throws<ArgumentException>(() => DataProtectionKeyStoreConfiguration.GetDataProtectionKeyStoreProvider());
        }
    }
}