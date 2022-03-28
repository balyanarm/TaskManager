using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.Tasks
{
    public interface ITaskService
    {
        public Task<TaskDto> GetTasksByNameAsync(string taskName);
        public Task<TaskDto> CreateTaskAsync(string taskName);
        public Task<TaskDto> DeleteTaskAsync(string taskName);

    }
}