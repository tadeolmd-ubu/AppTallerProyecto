using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTaller.Model;
using AppTaller.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace AppTaller.EF
{
    internal class efAppDbContext : DbContext
    {
        //por cada model se agrega aqui  un DbSet con el MISMO MOMBRE QUE LA CLASE
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<catRol> catRol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<catEmpresa> catEmpresa { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<TipoProducto> TipoProducto { get; set; }
        public DbSet<Producto> Producto { get; set; }

        //Cadena de conexion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=E4;Initial Catalog=dbTaller;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        }
        //igual aca que en el DbSet
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuario"); 
            modelBuilder.Entity<catRol>().ToTable("catRol");
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Direccion>().ToTable("Direccion");
            modelBuilder.Entity<catEmpresa>().ToTable("catEmpresa");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedor");
            modelBuilder.Entity<TipoProducto>().ToTable("TipoProducto");
            modelBuilder.Entity<Producto>().ToTable("Producto");
        }

    }
}
