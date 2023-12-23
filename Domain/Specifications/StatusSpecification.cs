using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Domain.Specifications
{
    public class StatusSpecification:Specification<Tasks>
    {
        public override bool IsSatisfiedBy(Tasks entity)
        {
            return entity.Status == _status;
        }

        private readonly Domain.Entities.TaskStatus? _status;

        public StatusSpecification(Domain.Entities.TaskStatus status)
        {
            this._status = status;
        }
    }
}
