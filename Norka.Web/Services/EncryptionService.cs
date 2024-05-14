using System.Security.Cryptography;
using System.Text;

namespace Norka.Web.Services;

public class EncryptionService : IEncryptionService
{
    public async Task<string> EncryptAsync(string key, string text)
    {
        var iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream())
            {
                await using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    await using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        await streamWriter.WriteAsync(text);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public async Task<string> DecryptAsync(string key, string text)
    {
        var iv = new byte[16];
        var buffer = Convert.FromBase64String(text);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(buffer);
        await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return await streamReader.ReadToEndAsync();
    }
}