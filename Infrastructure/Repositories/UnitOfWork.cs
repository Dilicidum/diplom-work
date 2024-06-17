using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly IVacancyRepository _taskRepository;
        private readonly ICriteriasRepository _criteriaRepository;
        private readonly ICandidatesRepository _candidatesRepository;
        private readonly IAnalysisRepository _analysisRepository;
        public UnitOfWork(ApplicationContext context, IVacancyRepository taskRepository, ICriteriasRepository criteriaRepository, ICandidatesRepository candidatesRepository,IAnalysisRepository analysisRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
            _criteriaRepository = criteriaRepository;
            _candidatesRepository = candidatesRepository;
            _analysisRepository = analysisRepository;
        }

        public IAnalysisRepository AnalysisRepository { 
                get
                {
                    return _analysisRepository;
                } 
            }

        public IVacancyRepository TasksRepository { 
                get
                {
                    return _taskRepository;
                } 
            }

        public ICriteriasRepository CriteriasRepository { 
                get
                {
                    return _criteriaRepository;
                } 
            }

        public ICandidatesRepository CandidatesRepository { 
                get
                {
                    return _candidatesRepository;
                } 
            }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
