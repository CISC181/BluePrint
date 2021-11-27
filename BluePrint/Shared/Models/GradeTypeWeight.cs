using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("GRADE_TYPE_WEIGHT")]
    [Index(nameof(GradeTypeCode), Name = "GRTW_GRTYP_FK_I")]
    public partial class GradeTypeWeight
    {
        public GradeTypeWeight()
        {
            Grades = new HashSet<Grade>();
        }

        [Key]
        [Column("SECTION_ID")]
        public int SectionId { get; set; }
        [Key]
        [Column("GRADE_TYPE_CODE")]
        [StringLength(2)]
        public string GradeTypeCode { get; set; }
        [Column("NUMBER_PER_SECTION")]
        public byte NumberPerSection { get; set; }
        [Column("PERCENT_OF_FINAL_GRADE")]
        public byte PercentOfFinalGrade { get; set; }
        [Required]
        [Column("DROP_LOWEST")]
        [StringLength(1)]
        public string DropLowest { get; set; }
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

        [ForeignKey(nameof(GradeTypeCode))]
        [InverseProperty(nameof(GradeType.GradeTypeWeights))]
        public virtual GradeType GradeTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(SectionId))]
        [InverseProperty("GradeTypeWeights")]
        public virtual Section Section { get; set; }
        [InverseProperty(nameof(Grade.GradeTypeWeight))]
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
