using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("GRADE_CONVERSION")]
    public partial class GradeConversion
    {
        [Key]
        [Column("LETTER_GRADE")]
        [StringLength(2)]
        public string LetterGrade { get; set; }
        [Column("GRADE_POINT", TypeName = "NUMBER(3,2)")]
        public decimal GradePoint { get; set; }
        [Column("MAX_GRADE")]
        public byte MaxGrade { get; set; }
        [Column("MIN_GRADE")]
        public byte MinGrade { get; set; }
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
    }
}
