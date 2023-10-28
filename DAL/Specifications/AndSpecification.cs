using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Specifications
{
    public class AndSpecification<T>:Specification<T>
    {
        private ISpecification<T> _leftSpecification;
        private ISpecification<T> _rightSpecification;

        public AndSpecification(ISpecification<T> leftSpecification,ISpecification<T> rightSpecification) 
        {
            _leftSpecification = leftSpecification;
            _rightSpecification = rightSpecification;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return _leftSpecification.IsSatisfiedBy(entity) && _rightSpecification.IsSatisfiedBy(entity);
        }
    }
}
