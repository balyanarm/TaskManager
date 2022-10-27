using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Repos;
using Domain.Services.Roles;
using Domain.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Domain.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskData> _taskRepository;
        private readonly Configs _configs;
        public TaskService(IRepository<TaskData> taskRepository,
            IOptions<Configs> configs)
        {
            _taskRepository = taskRepository;
            _configs = configs.Value;
        }
        public async Task<TaskDto> GetTasksByNameAsync(string taskName)
        {
            var task = await _taskRepository.Query.FirstOrDefaultAsync(t => t.Name == taskName);
            if (task == null)
            {
                return null;
            }
            return new TaskDto()
            {
                Id = task.Id,
                Name = task.Name,
                CreationTime = task.CreationTime
            };
        }

        public async Task<TaskDto> CreateTaskAsync(string taskName)
        {
            var task = await _taskRepository.AddAsync(new TaskData()
            {
                Name = taskName,
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow
            });

            return new TaskDto()
            {
                Id = task.Id,
                Name = task.Name,
                CreationTime = task.CreationTime
            };
        }

        public async Task<TaskDto> DeleteTaskAsync(string taskName)
        {
            var task = await _taskRepository.Query.FirstOrDefaultAsync(t => t.Name == taskName);
            if (task == null)
            {
                return null;
            }

            await _taskRepository.RemoveAsync(task);
            return new TaskDto()
            {
                Id = task.Id,
                Name = taskName,
                CreationTime = task.CreationTime
            };
        }

        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.Query
                .Select(t => new TaskDto()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        CreationTime = t.CreationTime
                    })
                .ToListAsync();

            return tasks;
        }
    }
}