namespace BanDoUong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Email")]
    public partial class Email
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string DiaChiEmail { get; set; }

        public string NoiDung { get; set; }
    }
}
