using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Domain.Entities.TaskStatus;

namespace Domain.Specifications
{
    public class VacancyByTypeAndStatusAndCategorySpecAndUserId:Specification<Vacancy>
    {
        public VacancyByTypeAndStatusAndCategorySpecAndUserId(string name, TaskType? type,TaskCategory? category, TaskStatus? status)
        {
            if (type.HasValue)
            {
                Query.Where(x=>x.TaskType== type);
            }

            if(category.HasValue)
            {
                Query.Where(x=>x.Category == category);
            }

            if(status.HasValue)
            {
                Query.Where(x=>x.Status == status);
            }

            if(!string.IsNullOrEmpty(name))
            {
                Query.Where(x=>x.Name.Contains(name));
            }
        }
    }
}
