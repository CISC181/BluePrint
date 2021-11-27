using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("STUDENT")]
    [Index(nameof(Zip), Name = "STU_ZIP_FK_I")]
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Key]
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }
        [Column("SALUTATION")]
        [StringLength(5)]
        public string Salutation { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Required]
        [Column("LAST_NAME")]
        [StringLength(25)]
        public string LastName { get; set; }
        [Column("STREET_ADDRESS")]
        [StringLength(50)]
        public string StreetAddress { get; set; }
        [Required]
        [Column("ZIP")]
        [StringLength(5)]
        public string Zip { get; set; }
        [Column("PHONE")]
        [StringLength(15)]
        public string Phone { get; set; }
        [Column("EMPLOYER")]
        [StringLength(50)]
        public string Employer { get; set; }
        [Column("REGISTRATION_DATE", TypeName = "DATE")]
        public DateTime RegistrationDate { get; set; }
        [Required]
        [Column("CREATED_BY")]
        [StringLength(30)]
        public string CreatedBy { get; set; }
        [Column("CREATED_DATE", TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Column("MODIFIED_BY")]
        [StringLength(30)]
        public string ModifiedBy { get; set; }
        [Column("MODIFIED_DATE", TypeName = "DATE")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(Zip))]
        [InverseProperty(nameof(Zipcode.Students))]
        public virtual Zipcode ZipNavigation { get; set; }
        [InverseProperty(nameof(Enrollment.Student))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
