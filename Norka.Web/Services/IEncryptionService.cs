namespace Norka.Web.Services;

public interface IEncryptionService
{
    public Task<string> EncryptAsync(string key, string text);
    public Task<string> DecryptAsync(string key, string text);
}