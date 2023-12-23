using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Interfaces
{
    public interface ITaskValidationService
    {
        public Task<bool> ValidateTaskExistence(int taskId);
    }
}
