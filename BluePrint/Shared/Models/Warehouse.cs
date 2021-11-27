using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("WAREHOUSE")]
    public partial class Warehouse
    {
        [Key]
        [Column("WAREHOUSE_ID", TypeName = "NUMBER")]
        public decimal WarehouseId { get; set; }
        [Column("WAREHOUSE_NAME")]
        [StringLength(20)]
        public string WarehouseName { get; set; }
    }
}
