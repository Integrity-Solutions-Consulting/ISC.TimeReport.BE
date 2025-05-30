using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Entity.Customers;
using isc.time.report.be.domain.Entity.Leaders;
using isc.time.report.be.domain.Entity.Menu;
using isc.time.report.be.domain.Entity.Projects;
using Microsoft.EntityFrameworkCore;

namespace isc.time.report.be.infrastructure.Database
{
    
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id_Customer");

                entity.Property(e => e.IdentificationType).HasColumnName("identification_type");
                entity.Property(e => e.IdentificationNumber).HasColumnName("identification_number");
                entity.Property(e => e.CommercialName).HasColumnName("commercial_name");
                entity.Property(e => e.CompanyName).HasColumnName("company_name");
                entity.Property(e => e.CellPhoneNumber).HasColumnName("cellphone_number");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<Leader>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id_Leader");
                entity.Property(e => e.IdentificationType).HasColumnName("identification_type");
                entity.Property(e => e.IdentificationNumber).HasColumnName("identification_number");
                entity.Property(e => e.Names).HasColumnName("names");
                entity.Property(e => e.Surnames).HasColumnName("surnames");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.CellPhoneNumber).HasColumnName("cell_phone_number");
                entity.Property(e => e.Position).HasColumnName("position");
                entity.Property(e => e.PersonalEmail).HasColumnName("personal_email");
                entity.Property(e => e.CorporateEmail).HasColumnName("corporate_email");
                entity.Property(e => e.HomeAddress).HasColumnName("home_address");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("users_id");

                entity.Property(e => e.email).HasColumnName("username");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<Rols>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("rols_id");

                entity.Property(e => e.RolName).HasColumnName("rol_name");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<UsersRols>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("users_rols_id");

                entity.Property(e => e.UsersId).HasColumnName("users_id");
                entity.Property(e => e.RolsId).HasColumnName("rols_id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");


                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UsersRols)
                      .HasForeignKey(ur => ur.UsersId);

                entity.HasOne(ur => ur.Rols)
                      .WithMany(r => r.UsersRols)
                      .HasForeignKey(ur => ur.RolsId);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("menu_id");

                entity.Property(e => e.NombreMenu).HasColumnName("nombre_menu");
                entity.Property(e => e.RutaMenu).HasColumnName("ruta_menu");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<MenuRols>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("menu_rols_id");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");
                entity.Property(e => e.RolsId).HasColumnName("rols_id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(ur => ur.Menu)
                      .WithMany(u => u.MenusRols)
                      .HasForeignKey(ur => ur.MenuId);

                entity.HasOne(ur => ur.Rols)
                      .WithMany(r => r.MenuRols)
                      .HasForeignKey(ur => ur.RolsId);

            });



            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Leader> People { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rols> Rols { get; set; }
        public DbSet<UsersRols> Users_Rols { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuRols> Menu_Rols { get; set; }
    }
}
