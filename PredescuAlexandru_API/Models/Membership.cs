using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PredescuAlexandru_API.Models
{
    public class Membership
    {
        [Key]
        public Guid IdMembership { get; set; }

        [ForeignKey("IdMember")]
        public Guid IdMember { get; set; }

        [ForeignKey("MembershipIdType")]
        public Guid IdMembershipType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Level { get; set; }
    }
}
