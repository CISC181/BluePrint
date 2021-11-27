using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("ADDRESS")]
    public partial class Address
    {
        [Key]
        [Column("ADDRESS_ID", TypeName = "NUMBER")]
        public decimal AddressId { get; set; }
    }
}
