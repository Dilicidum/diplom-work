using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAnalysisService
    {
        
        public Task RecordAnalysis(int vacancyId, List<int> candidatesId);

        Task<IEnumerable<string>> GetAnalysisForVacancy(int vacancyId);

    }
}
