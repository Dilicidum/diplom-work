using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications
{
    public class TaskTypeSpecification:Specification<Tasks>
    {

        private TaskType _taskType { get; }

        public TaskTypeSpecification(TaskType taskType)
        {
            _taskType = taskType;
        }

        public override bool IsSatisfiedBy(Tasks entity)
        {
            return entity.TaskType == _taskType;
        }
    }
}
