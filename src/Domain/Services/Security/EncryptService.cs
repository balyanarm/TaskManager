namespace Domain.Services.Security
{
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string text, string key)
        {            
            return StringCipher.HashString(text, key);
        }
    }
}
