namespace Domain.Services.Users
{
    public class UserWithPasswordHashDto : UserDto
    {
        public string PasswordHash { get; set; }
    }
}
