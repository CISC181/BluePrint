using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Keyless]
    public partial class VStudentFinalGrade
    {
        [Column("STUDENT_ID")]
        public int? StudentId { get; set; }
        [Column("SECTION_ID")]
        public int? SectionId { get; set; }
        [Column("FINALGRADE", TypeName = "NUMBER")]
        public decimal? Finalgrade { get; set; }
    }
}
