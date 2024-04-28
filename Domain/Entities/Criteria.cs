using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Criteria
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Tasks? Vacancy { get; set; }

        public int? VacancyId { get; set; }

        public double VacancyWeight { get; set; } = 0;

        public int Order {  get; set; }

        public List<CandidateCriteria>? CandidateCriterias { get; set; }

    }
}
