namespace EngineBay.DataProtection
{
    public enum DataProtectionKeyStoreTypes
    {
        FileSystem, // for testing in memory or local development, not recommended for prod
        Redis,
    }
}
