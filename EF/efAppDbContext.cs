using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTaller.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace AppTaller.EF
{
    internal class efAppDbContext : DbContext
    {
        public DbSet<catRol> catRol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=E4;Initial Catalog=dbTaller;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuario"); // tu tabla es singular
        }

    }
}
