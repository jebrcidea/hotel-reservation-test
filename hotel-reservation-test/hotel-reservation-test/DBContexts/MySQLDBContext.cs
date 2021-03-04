using hotel_reservation_test.Models;
using hotel_reservation_test.Models.Database;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.DBContexts
{
    public class MySQLDBContext : DbContext
    {
        public DbSet<Test> Test { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Configurations> Configurations { get; set; }

        public MySQLDBContext (DbContextOptions<MySQLDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  
            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<Hotel>().ToTable("Hotel");
            modelBuilder.Entity<Rooms>().ToTable("Rooms");
            modelBuilder.Entity<Reservations>().ToTable("Reservations");
            modelBuilder.Entity<Configurations>().ToTable("Configurations");

            // Configure Primary Keys  
            modelBuilder.Entity<Hotel>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Rooms>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Reservations>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Configurations>().HasKey(ug => ug.id).HasName("PRIMARY KEY");

            // Configure indexes  
            modelBuilder.Entity<Test>().HasIndex(p => p.testoru).IsUnique();

            // Configure relationships  
            modelBuilder.Entity<Rooms>().HasOne<Hotel>().WithMany().HasPrincipalKey(ug => ug.id).HasForeignKey(u => u.idHotel).OnDelete(DeleteBehavior.NoAction).HasConstraintName("idHotel");
            modelBuilder.Entity<Reservations>().HasOne<Rooms>().WithMany().HasPrincipalKey(ug => ug.id).HasForeignKey(u => u.idRoom).OnDelete(DeleteBehavior.NoAction).HasConstraintName("idRoom");
        }
    }
}
