using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Services.Abstractions.DTO
{
    public class TaskInputModel
    {
         public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TaskCategory Category { get; set; }

        [Required]
        public Domain.Entities.TaskStatus Status { get; set; }

        public int? BaseTaskId { get;set; }

        [Required]
        public TaskType TaskType { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
