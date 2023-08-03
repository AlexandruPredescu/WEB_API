using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PredescuAlexandru_API.Models
{
    public class CodeSnippet
    {
        [Key]
        [JsonIgnore]
        public Guid IdCodeSnippet { get; set; }

        public string? Title { get; set; }

        public string? ContentCode { get; set; }


        [ForeignKey("IdMember")]
        public Guid IdMember { get; set; }

        public int? Revision { get; set; }

        public DateTime? DateTimeAdded { get; set; }

        public bool? IsPublished { get; set; }
    }
}
