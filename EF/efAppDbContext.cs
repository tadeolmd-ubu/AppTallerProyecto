using AppTaller.Model;
using System;
using System.Configuration;
using System.Linq;
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
        public DbSet<catMarca> catMarcas { get; set; }
        public DbSet<catAlmacen> catAlmacen { get; set; }
        public DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public DbSet<ReferenciaMovimiento> ReferenciaMovimiento { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<MovimientoInventario> MovimientoInventario { get; set; }
        public DbSet<Presupuesto> Presupuesto { get; set; }
        public DbSet<PresupuestoDetalle> PresupuestoDetalle { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<VentaDetalle> VentaDetalle { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = ConfigurationManager.ConnectionStrings["DbTaller"].ConnectionString;
            optionsBuilder.UseSqlServer(connString);
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
            modelBuilder.Entity<catMarca>().ToTable("catMarca");
            modelBuilder.Entity<catAlmacen>().ToTable("catAlmacen");
            modelBuilder.Entity<TipoMovimiento>().ToTable("TipoMovimiento");
            modelBuilder.Entity<ReferenciaMovimiento>().ToTable("ReferenciaMovimiento");
            modelBuilder.Entity<Inventario>().ToTable("Inventario");
            modelBuilder.Entity<MovimientoInventario>().ToTable("MovimientoInventario");
            modelBuilder.Entity<Presupuesto>().ToTable("Presupuesto");
            modelBuilder.Entity<PresupuestoDetalle>().ToTable("PresupuestoDetalle");
            modelBuilder.Entity<Venta>().ToTable("Venta");
            modelBuilder.Entity<VentaDetalle>().ToTable("VentaDetalle");
        }

    }
}
