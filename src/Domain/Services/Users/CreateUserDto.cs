﻿namespace Domain.Services.Users
{
    public class CreateUserDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        
        public  string RoleName { get; set; }
    }
}
