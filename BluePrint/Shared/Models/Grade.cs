using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("GRADE")]
    [Index(nameof(SectionId), nameof(GradeTypeCode), Name = "GR_GRTW_FK_I")]
    public partial class Grade
    {
        [Key]
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }
        [Key]
        [Column("SECTION_ID")]
        public int SectionId { get; set; }
        [Key]
        [Column("GRADE_TYPE_CODE")]
        [StringLength(2)]
        public string GradeTypeCode { get; set; }
        [Key]
        [Column("GRADE_CODE_OCCURRENCE", TypeName = "NUMBER(38)")]
        public decimal GradeCodeOccurrence { get; set; }
        [Column("NUMERIC_GRADE")]
        public byte NumericGrade { get; set; }
        [Column("COMMENTS")]
        [StringLength(2000)]
        public string Comments { get; set; }
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

        [ForeignKey("SectionId,GradeTypeCode")]
        [InverseProperty("Grades")]
        public virtual GradeTypeWeight GradeTypeWeight { get; set; }
        [ForeignKey("StudentId,SectionId")]
        [InverseProperty(nameof(Enrollment.Grades))]
        public virtual Enrollment S { get; set; }
    }
}
