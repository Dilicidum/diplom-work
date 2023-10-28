using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications
{
    public class TaskCategorySpecification:Specification<Tasks>
    {

        private TaskCategory _taskCategory { get; }

        public TaskCategorySpecification(TaskCategory taskCategory)
        {
            _taskCategory = taskCategory;
        }

        public override bool IsSatisfiedBy(Tasks entity)
        {
            return entity.Category == _taskCategory;
        }
    }
}
