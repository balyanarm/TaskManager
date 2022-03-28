namespace Domain.Core
{
    public class Configs
    {
        public virtual string JwtKey { get; set; }
        public virtual string JwtIssuer { get; set; }
        public virtual string JwtAudience { get; set; }
        public virtual int JwtDurationInMinutes { get; set; }
        
        public  virtual string Encryptkey { get; set; }
    }
}