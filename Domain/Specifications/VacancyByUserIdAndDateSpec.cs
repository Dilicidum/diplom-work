﻿using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class VacancyByUserIdAndDateSpec:Specification<Vacancy>
    {
        public VacancyByUserIdAndDateSpec(string userId, DateTime dateTime) {
            Query.Where(x => x.UserId == userId && x.DueDate == dateTime);
        }
    }
}
