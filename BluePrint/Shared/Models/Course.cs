using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("COURSE")]
    [Index(nameof(Prerequisite), Name = "CRSE_CRSE_FK_I")]
    public partial class Course
    {
        public Course()
        {
            InversePrerequisiteNavigation = new HashSet<Course>();
            Sections = new HashSet<Section>();
        }

        [Key]
        [Column("COURSE_NO")]
        public int CourseNo { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("COST", TypeName = "NUMBER(9,2)")]
        public decimal? Cost { get; set; }
        [Column("PREREQUISITE")]
        public int? Prerequisite { get; set; }
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

        [ForeignKey(nameof(Prerequisite))]
        [InverseProperty(nameof(Course.InversePrerequisiteNavigation))]
        public virtual Course PrerequisiteNavigation { get; set; }
        [InverseProperty(nameof(Course.PrerequisiteNavigation))]
        public virtual ICollection<Course> InversePrerequisiteNavigation { get; set; }
        [InverseProperty(nameof(Section.CourseNoNavigation))]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
