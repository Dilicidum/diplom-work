using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Services.Abstractions.DTO
{
    public class UserRegistrationModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]

        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
