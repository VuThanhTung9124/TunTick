namespace Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [Key]
        public int product_id { get; set; }

        [Required]
        [StringLength(150)]
        public string name { get; set; }

        public string description { get; set; }

        public decimal price { get; set; }

        public decimal? old_price { get; set; }

        [StringLength(255)]
        public string thumbnail { get; set; }

        [StringLength(100)]
        public string category { get; set; }

        public bool status { get; set; }
    }
}
