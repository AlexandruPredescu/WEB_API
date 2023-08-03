using System.ComponentModel.DataAnnotations;

namespace PredescuAlexandru_API.Models
{
    public class MembershipType
    {
        [Key]
        public Guid IdMembershipType { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? SubcriptionLenghtInMonths { get; set; }
    }
}
