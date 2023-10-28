using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications
{
    public abstract class Specification<T>:ISpecification<T>
    {

        public abstract bool IsSatisfiedBy(T entity);

        public Specification<T> AndSpecification(Specification<T> specification)
        {
            return new AndSpecification<T>(this,specification);
        }
    }
}
