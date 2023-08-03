using System.ComponentModel.DataAnnotations;

namespace PredescuAlexandru_API.Models
{
    public class Member
    {
        [Key]
        public Guid IdMember { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? Position { get; set; }

        public string? Description { get; set; }

        public string? Resume { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
