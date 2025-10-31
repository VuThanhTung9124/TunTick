namespace BanDoUong.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EmailDb : DbContext
    {
        public EmailDb()
            : base("name=EmailDb")
        {
        }

        public virtual DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
