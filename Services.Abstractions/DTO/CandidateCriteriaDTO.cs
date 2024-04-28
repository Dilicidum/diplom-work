using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.DTO
{
    public class CandidateCriteriaDTO
    {
        public int CriteriaId { get; set; }

        public int CandidateId { get; set; }

        public double Value { get; set; }
    }
}
