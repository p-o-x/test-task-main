using System.Text.Json.Serialization;

namespace task.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string PhoneNumber { get; set; }
        public string? Additional { get; set; }

        [JsonIgnore]
        public Office Office { get; set; }
    }
}
