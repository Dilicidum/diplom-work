using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task Save();

        public IVacancyRepository TasksRepository { get; }

        public ICriteriasRepository CriteriasRepository { get; }

        public ICandidatesRepository CandidatesRepository { get; }

        public IAnalysisRepository AnalysisRepository { get; }
    }
}
