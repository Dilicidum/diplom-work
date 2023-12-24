using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class SubTasksByBaseTaskIdSpec: Specification<Tasks>
    {
        public SubTasksByBaseTaskIdSpec(int taskId) {
            Query.Where(x=>x.BaseTaskId == taskId);
        }
    }
}
