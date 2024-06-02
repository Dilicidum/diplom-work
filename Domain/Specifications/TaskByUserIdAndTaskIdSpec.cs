using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Domain.Specifications
{
    public class TaskByUserIdAndTaskIdSpec:Specification<Tasks>,ISingleResultSpecification<Tasks>
    {
        public TaskByUserIdAndTaskIdSpec(string userId, int taskId)
        {
            Query.Where(x => x.UserId == userId && x.Id == taskId);
        }
    }
}
