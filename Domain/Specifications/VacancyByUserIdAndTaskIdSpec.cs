﻿using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Domain.Specifications
{
    public class VacancyByUserIdAndTaskIdSpec:Specification<Vacancy>,ISingleResultSpecification<Vacancy>
    {
        public VacancyByUserIdAndTaskIdSpec(string userId, int taskId)
        {
            Query.Where(x => x.Id == taskId);
        }
    }
}