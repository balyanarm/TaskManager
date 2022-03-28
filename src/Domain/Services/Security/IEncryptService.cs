namespace Domain.Services.Security
{
    public interface IEncryptService
    {
        string Encrypt(string text, string key);
    }
}
