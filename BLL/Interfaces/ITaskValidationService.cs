using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITaskValidationService
    {
        public Task<bool> ValidateTaskExistence(int taskId);
    }
}
