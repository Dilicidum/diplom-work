using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications;

public class TaskByTaskIdWithSubTasksSpec: Specification<Tasks>, ISingleResultSpecification<Tasks>
{
    public TaskByTaskIdWithSubTasksSpec(int taskId) {
        Query.Where(x => x.Id == taskId).Include(x=>x.SubTasks);
    }


}
