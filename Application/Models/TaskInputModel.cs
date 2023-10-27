using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace API.Models
{
    public class TaskInputModel
    {
         public int Id { get; set; }
        
         public string Name { get; set; }

        public string Description { get; set; }

        public TaskCategory Category { get; set; }

        public DAL.Models.TaskStatus Status { get; set; }

        public int? BaseTaskId { get;set; }

        public TaskType TaskType { get; set; }


        public string? UserId { get; set; }

        public DateTime DueDate { get; set; }
    }
}
