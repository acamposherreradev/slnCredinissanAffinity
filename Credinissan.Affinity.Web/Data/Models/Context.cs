using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Credinissan.Affinity.Web.Data.DataObject;

namespace Credinissan.Affinity.Web.Data.Models
{
    public class Context : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Rol>().ToTable("Roles");
            modelBuilder.Entity<UsuarioRol>().ToTable("UsuariosRoles");
            //Cotizacion
            modelBuilder.Entity<Cotizacion>().ToTable("Cotizaciones");
            modelBuilder.Entity<Marca>().ToTable("Marcas");
            modelBuilder.Entity<Modelo>().ToTable("Modelos");
            modelBuilder.Entity<DataObject.Version>().ToTable("Versiones");
            modelBuilder.Entity<DataObject.Distribuidor>().ToTable("Distribuidores");
            modelBuilder.Entity<DataObject.Sucursal>().ToTable("Sucursales");

            modelBuilder.Entity<DataObject.Comuna>().ToTable("Comunas");
            modelBuilder.Entity<DataObject.Region>().ToTable("Regiones");
            modelBuilder.Entity<DataObject.Contratante>().ToTable("Contratantes");
            modelBuilder.Entity<DataObject.Solicitud>().ToTable("Solicitudes");

            //modelBuilder.Entity<Tarea>().Property(obj => obj.TipoTarea).HasColumnName("IdTipoTarea");
            //modelBuilder.Entity<Clase>().Property(obj => obj.Precio).HasPrecision(12, 4);
            //modelBuilder.Entity <Bolsa>().Property(obj => obj.Factor).HasPrecision(10, 6);


            //modelBuilder.Entity<Cliente>().Property(p => p.Imagen).HasColumnType("image");

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<DataObject.Version> Versiones { get; set; }
        public DbSet<Distribuidor> Distribuidores { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Comuna> Comunas { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Contratante> Contratantes { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }


    }
}