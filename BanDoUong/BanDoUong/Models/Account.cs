namespace BanDoUong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        

        [Key]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string HoVaTen { get; set; }

        [StringLength(20)]
        public string SoDienThoai { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

public Account()
        {
        }

    public Account(string email, string hoVaTen, string soDienThoai, string matKhau)
        {
            Email = email;
            HoVaTen = hoVaTen;
            SoDienThoai = soDienThoai;
            MatKhau = matKhau;
        }

        
    }
}
