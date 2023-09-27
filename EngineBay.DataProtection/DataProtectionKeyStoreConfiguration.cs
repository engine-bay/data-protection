namespace EngineBay.DataProtection
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

    public static class DataProtectionKeyStoreConfiguration
    {
        public static void AddKeyStoreProvider(IServiceCollection services)
        {
            var dataProtectionKeyStoreProvider = GetDataProtectionKeyStoreProvider();
            var dataProtectionNamespace = GetDataProtectionNamespace();
            var keyLifetime = GetKeyLifetime();

            switch (dataProtectionKeyStoreProvider)
            {
                case DataProtectionKeyStoreTypes.FileSystem:
                    services.AddDataProtection()
                      .SetDefaultKeyLifetime(TimeSpan.FromDays(keyLifetime))
                      .SetApplicationName(dataProtectionNamespace)
                      .PersistKeysToFileSystem(new DirectoryInfo(@"./tmp"));
                    break;
                case DataProtectionKeyStoreTypes.Redis:
                    var connectionString = GetRedisConnectionString();

                    var config = new ConfigurationOptions
                    {
                        EndPoints =
              {
                  { connectionString },
              },
                        ReconnectRetryPolicy = new ExponentialRetry(5000),
                    };
                    var redis = ConnectionMultiplexer.Connect(config);

                    services.AddDataProtection()
                      .SetDefaultKeyLifetime(TimeSpan.FromDays(keyLifetime))
                      .SetApplicationName(dataProtectionNamespace)
                      .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
                    break;
                default:
                    throw new ArgumentException($"Unhandled {EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER} configuration of '{dataProtectionKeyStoreProvider}'.");
            }
        }

        public static string GetDataProtectionNamespace()
        {
            var namespaceString = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONNAMESPACE);

            if (!string.IsNullOrEmpty(namespaceString))
            {
                return namespaceString;
            }

            Console.WriteLine($"Warning: '{EnvironmentVariableConstants.DATAPROTECTIONNAMESPACE} was not configured. Defaulting to '{DefaultDataProtectionConstants.Namespace}'.");

            return DefaultDataProtectionConstants.Namespace;
        }

        public static string GetRedisConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONREDISCONNECTIONSTRING);

            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            throw new ArgumentException($"Invalid {EnvironmentVariableConstants.DATAPROTECTIONREDISCONNECTIONSTRING} configuration.");
        }

        public static int GetKeyLifetime()
        {
            var keyLifetimeString = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS);

            var keyLifetime = DefaultDataProtectionConstants.KeyLifetime;

            if (int.TryParse(keyLifetimeString, out keyLifetime))
            {
                if (keyLifetime < 7)
                { // minimum can't be less than 7 according to the docs.
                    throw new ArgumentException($"Invalid {EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS} configuration. Cannot be less than 7 days.");
                }

                return keyLifetime;
            }

            Console.WriteLine($"Warning: '{EnvironmentVariableConstants.DATAPROTECTIONKEYLIFETIMEDAYS} was not configured. Defaulting to '{DefaultDataProtectionConstants.KeyLifetime}'.");

            return DefaultDataProtectionConstants.KeyLifetime;
        }

        public static DataProtectionKeyStoreTypes GetDataProtectionKeyStoreProvider()
        {
            var dataProtectionKeyStoreProviderString = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER);

            if (string.IsNullOrEmpty(dataProtectionKeyStoreProviderString))
            {
                Console.WriteLine($"Warning: {EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER} not configured, defaulting to {DataProtectionKeyStoreTypes.FileSystem} key store provider. This is not intended for production usage.");
                return DataProtectionKeyStoreTypes.FileSystem;
            }

            var dataProtectionKeyStoreProvider = (DataProtectionKeyStoreTypes)Enum.Parse(typeof(DataProtectionKeyStoreTypes), dataProtectionKeyStoreProviderString);

            if (!Enum.IsDefined(typeof(DataProtectionKeyStoreTypes), dataProtectionKeyStoreProvider) | dataProtectionKeyStoreProvider.ToString().Contains(',', StringComparison.InvariantCulture))
            {
                Console.WriteLine($"Warning: '{dataProtectionKeyStoreProviderString}' is not a valid {EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER} configuration option. Valid options are: ");
                foreach (string name in Enum.GetNames(typeof(DataProtectionKeyStoreTypes)))
                {
                    Console.Write(name);
                    Console.Write(", ");
                }

                throw new ArgumentException($"Invalid {EnvironmentVariableConstants.DATAPROTECTIONKEYSTOREPROVIDER} configuration.");
            }

            return dataProtectionKeyStoreProvider;
        }
    }
}
