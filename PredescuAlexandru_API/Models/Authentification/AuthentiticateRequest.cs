using System.ComponentModel.DataAnnotations;

namespace PredescuAlexandru_API.Models.Authentification
{
    public class AuthentiticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
