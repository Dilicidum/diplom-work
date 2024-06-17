using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vacancy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TaskCategory Category { get; set; }

        public TaskStatus Status { get; set; }

        public TaskType TaskType { get; set; }

        public string UserId { get; set; }

        public DateTime DueDate { get; set; }

        public IdentityUser User { get; set; }

        public List<Analysis> Analyses { get; set; }

        public List<Criteria> Criterias { get; set; }

        public List<Candidate> Candidates { get; set; }
    }

    public enum TaskType
    {
        Task,//0
        SubTask//1
    }

    public enum TaskCategory
    {
        Development,
        Sales,
        Finances,
        Security,
        Analytics
    }

    public enum TaskStatus
    {
        None,//0
        Progress,//1
        Done,//2
        Rejected//3
    }
}
