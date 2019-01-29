using Admin.MODELS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.DAL
{
    public class MyContext:DbContext
    {
        public MyContext():base(nameOrConnectionString:"name=MyCon")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().Property(x => x.TaxRate).HasPrecision(3, scale: 2);
        }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
