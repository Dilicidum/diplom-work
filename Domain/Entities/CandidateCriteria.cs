using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CandidateCriteria
{
    public int CandidateId { get; set; }
    public Candidate Candidate { get; set; }
    public int CriteriaId { get; set; }
    public Criteria Criteria { get; set; }

    public double Value { get; set; }
}
}
