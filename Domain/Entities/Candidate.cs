using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Candidate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Tasks Vacancy { get; set; }

        public int VacancyId { get; set; }

        public List<CandidateCriteria> CandidateCriterias { get; set; }

    }
}
