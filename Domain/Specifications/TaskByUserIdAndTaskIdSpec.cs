using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class TaskByUserIdAndTaskIdSpec:Specification<Tasks>,ISingleResultSpecification<Tasks>
    {
        public TaskByUserIdAndTaskIdSpec(string userId, int taskId)
        {
            Query.Where(x => x.UserId == userId && x.Id == taskId).Include(x=>x.SubTasks);
        }
    }
}
