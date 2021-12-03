using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("PERSON")]
    public partial class Person
    {
        [Key]
        [Column("PERSON_ID", TypeName = "NUMBER")]
        public decimal PersonId { get; set; }
        [Column("PERSON_FIRST_NAME")]
        [StringLength(20)]
        public string PersonFirstName { get; set; }
        [Column("PERSON_LAST_NAME")]
        [StringLength(50)]
        public string PersonLastName { get; set; }
    }
}
