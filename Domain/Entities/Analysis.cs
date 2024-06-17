using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Analysis
    {
        public Vacancy Vacancy { get; set; }

        public int Id { get; set; }

        public int VacancyId { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }
    }
}
