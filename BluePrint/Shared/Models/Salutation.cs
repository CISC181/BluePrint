using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("SALUTATION")]
    public partial class Salutation
    {
        public Salutation()
        {
            Students = new HashSet<Student>();
        }

        [Key]
        [Column("SALUTATION_ID", TypeName = "NUMBER")]
        public decimal SalutationId { get; set; }
        [Required]
        [Column("SALUTATION")]
        [StringLength(5)]
        public string Salutation1 { get; set; }

        [InverseProperty(nameof(Student.Salutation))]
        public virtual ICollection<Student> Students { get; set; }
    }
}
