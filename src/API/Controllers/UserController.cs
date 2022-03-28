using System;
using System.Threading.Tasks;
using Domain.Services.Auth;
using Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUsersService _usersService;
        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string userName)
        {
            var result = await _usersService.GetUserByNameAsync(userName);
            if (result == null)
            {
                return NotFound($"User '{userName}' not found.");
            }
            return Ok(result);
        }
        
              
        [HttpPost]
        public async Task<IActionResult> Post(CreateUserDto userDto)
        {
            var result = await _usersService.CreateUserAsync(userDto);
            return Ok(result);
        }
        
              
        [HttpDelete]
        public async Task<IActionResult> Delete(string userName)
        {
            var result = await _usersService.GetUserByNameAsync(userName);
            if (result == null)
            {
                return NotFound($"User '{userName}' not found.");
            }

            var userDto = await _usersService.DeleteUserAsync(userName);
            return Ok(userDto);
        }
    }
}