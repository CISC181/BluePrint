using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrint.Shared.DTO
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public decimal? SalutationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Employer { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public String Salutation { get; set; }
    }
}
