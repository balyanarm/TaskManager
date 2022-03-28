using System.Threading.Tasks;

namespace Domain.Services.Auth
{
    public interface IAuthService
    {
        public Task<LoginResultDto> LoginAsync(LoginDto loginDto);
        
    }
}