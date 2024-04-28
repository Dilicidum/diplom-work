using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.DTO
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CandidateCriteriaDTO> CandidateCriterias { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
