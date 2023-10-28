using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DAL.Specifications
{
    public class StatusSpecification:Specification<Tasks>
    {
        public override bool IsSatisfiedBy(Tasks entity)
        {
            return entity.Status == _status;
        }

        private readonly DAL.Models.TaskStatus? _status;

        public StatusSpecification(DAL.Models.TaskStatus status)
        {
            this._status = status;
        }
    }
}
