using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Company
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Industry { get; set; }

        public Company()
        {
        }
    }
}
