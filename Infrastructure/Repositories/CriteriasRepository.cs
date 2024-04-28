using Ardalis.Specification.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CriteriasRepository : RepositoryBase<Criteria>, ICriteriasRepository
    {
        private readonly ApplicationContext _context;

        public CriteriasRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
