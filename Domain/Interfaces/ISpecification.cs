using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISpecification<T>
    {
        public bool IsSatisfiedBy(T entity);

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this,specification);
        }
    }
}
