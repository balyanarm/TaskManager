using System.Threading.Tasks;
using Domain.Services.Tasks;
using Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class 
        TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(string taskName)
        {
            var result = await _taskService.GetTasksByNameAsync(taskName);
            if (result == null)
            {
                return NotFound($"Task '{taskName}' not found.");
            }
            return Ok(result);
        }
        
              
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TaskDto taskDto)
        {
            var result = await _taskService.CreateTaskAsync(taskDto.Name);
            return Ok(result);
        }
        
              
        [HttpDelete]
        public async Task<IActionResult> Delete(string taskName)
        {
            var result = await _taskService.GetTasksByNameAsync(taskName);
            if (result == null)
            {
                return NotFound($"Task '{taskName}' not found.");
            }

            var taskDto = await _taskService.DeleteTaskAsync(taskName);
            return Ok(taskDto);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskService.GetAllTasksAsync());
        }

    }
}