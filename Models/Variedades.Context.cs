using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Variedades.Models
{    
    public partial class DbmejiaEntities : DbContext
    {
        public DbmejiaEntities() : base("DbMejia")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbmejiaEntities, Variedades.Migrations.Configuration>());      
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Especificacion_producto> Especificacion_producto { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<DetalleProveedor> DetalleProveedor { get; set; }
        public virtual DbSet<Especificacion_pedido> Especificacion_pedido { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<Producto_importado> Producto_Importado { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<Proveedor_producto> Proveedor_producto { get; set; }
        public virtual DbSet<Telefono> Telefono { get; set; }

        //Useraccounts model
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Disable pluralization convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Delete cascade or null if case
            modelBuilder.Entity<Producto>()
                .HasMany(c => c.Especificaciones_producto)
                .WithOptional(x => x.Producto)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Cliente>()
               .HasMany(c => c.Ventas)
               .WithOptional(x => x.Cliente)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Proveedor_producto>()
                .HasMany(x => x.Especificacion_Productos)
                .WithOptional(x => x.Proveedor_Producto)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOptional(x => x.cliente)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Telefonos)
                .WithOptional(x => x.Cliente)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Venta>()
                .HasMany(c => c.Pagos)
                .WithOptional(c => c.Venta)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Pedido>()
                .HasMany(c => c.Especificaciones_pedido)
                .WithOptional(c => c.Pedido)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DetalleProveedor>()
               .HasMany(c => c.Producto_Importados)
               .WithOptional(c => c.DetalleProveedor)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Producto>()
               .HasMany(c => c.Especificaciones_producto)
               .WithOptional(c => c.Producto)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Venta>()
              .HasMany(c => c.Especificaciones_producto)
              .WithOptional(c => c.Venta)
              .WillCascadeOnDelete(true);


            modelBuilder.Entity<Proveedor>()
                .HasMany(c => c.Productos)
                .WithOptional(c => c.Proveedor)
                .WillCascadeOnDelete(true);

          

        }
    }
}
