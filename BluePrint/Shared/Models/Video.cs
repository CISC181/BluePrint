using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BluePrint.Shared.Models
{
    [Table("VIDEO")]
    public partial class Video
    {
        [Key]
        [Column("VIDEO_ID", TypeName = "NUMBER")]
        public decimal VideoId { get; set; }
        [Column("PRICE", TypeName = "NUMBER(9,2)")]
        public decimal? Price { get; set; }
        [Column("ACTIVE_FLG")]
        [StringLength(1)]
        public string ActiveFlg { get; set; }
    }
}
