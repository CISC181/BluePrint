using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("INSTRUCTOR")]
    [Index(nameof(Zip), Name = "INST_ZIP_FK_I")]
    public partial class Instructor
    {
        public Instructor()
        {
            Sections = new HashSet<Section>();
        }

        [Key]
        [Column("INSTRUCTOR_ID")]
        public int InstructorId { get; set; }
        [Column("SALUTATION")]
        [StringLength(5)]
        public string Salutation { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        [StringLength(25)]
        public string LastName { get; set; }
        [Column("STREET_ADDRESS")]
        [StringLength(50)]
        public string StreetAddress { get; set; }
        [Column("ZIP")]
        [StringLength(5)]
        public string Zip { get; set; }
        [Column("PHONE")]
        [StringLength(15)]
        public string Phone { get; set; }
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
        [InverseProperty(nameof(Zipcode.Instructors))]
        public virtual Zipcode ZipNavigation { get; set; }
        [InverseProperty(nameof(Section.Instructor))]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
