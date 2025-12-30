namespace SharedScriptsApi.Interfaces
{
    public interface ICryptographyProvider
    {
        string? Encrypt(string value);
        string? Encrypt(string value, string password, string salt);
        string? Decrypt(string value);
        string? Decrypt(string value, string password, string salt);
        byte[] Decrypt(string password, string salt, byte[] buffer, int offset, int count);
        byte[] Decrypt(byte[] key, byte[] buffer, int offset, int count);
        byte[] GetKey(string password, string salt);
    }
}
