using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TaskCategory Category { get; set; }

        public TaskStatus Status { get; set; }

        public List<Tasks> SubTasks { get; set; }

        public int? BaseTaskId { get;set; }

        public TaskType TaskType { get; set; }

        public string UserId { get; set; }

        public DateTime DueDate { get; set; }

        public IdentityUser User { get; set; }

    }

    public enum TaskType
    {
        Task,//0
        SubTask//1
    }

    public enum TaskCategory
    {
        Fitness,//0
        Food,//1
        Work,//2
        University,//3
        Health,//4
        Friends,//5
        Family//6
    }

    public enum TaskStatus
    {
        None,//0
        Progress,//1
        Done,//2
        Rejected//3
    }
}
