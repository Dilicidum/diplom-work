using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Services.Abstractions.DTO
{
    public class TaskInputModel
    {
         public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public TaskCategory Category { get; set; }

        public Domain.Entities.TaskStatus Status { get; set; }

        public int? BaseTaskId { get;set; }

        public TaskType TaskType { get; set; }

        public string? UserId { get; set; }

        public DateTime DueDate { get; set; }

        public List<CriteriaDto>? Criterias { get; set; }


    }
}
