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
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Configurations> Configurations { get; set; }

        public MySQLDBContext (DbContextOptions<MySQLDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  
            modelBuilder.Entity<Hotel>().ToTable("Hotel");
            modelBuilder.Entity<Rooms>().ToTable("Rooms");
            modelBuilder.Entity<Bookings>().ToTable("Bookings");
            modelBuilder.Entity<Configurations>().ToTable("Configurations");

            // Configure Primary Keys  
            modelBuilder.Entity<Hotel>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Rooms>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Bookings>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
            modelBuilder.Entity<Configurations>().HasKey(ug => ug.id).HasName("PRIMARY KEY");
        }
    }
}
