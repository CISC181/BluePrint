using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("TABLE1")]
    public partial class Table1
    {
        [Key]
        [Column("COLUMN1")]
        [StringLength(20)]
        public string Column1 { get; set; }
        [Column("COLUMN2")]
        [StringLength(20)]
        public string Column2 { get; set; }
        [Column("COLUMN3")]
        [StringLength(20)]
        public string Column3 { get; set; }
        [Column("COLUMN4")]
        [StringLength(20)]
        public string Column4 { get; set; }
        [Column("COLUMN5")]
        [StringLength(20)]
        public string Column5 { get; set; }
    }
}
