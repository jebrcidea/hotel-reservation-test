using hotel_reservation_test.Controllers;
using hotel_reservation_test.DBContexts;
using hotel_reservation_test.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace hotel_reservation_unit_testing
{
    public class UnitTestRooms
    {
        /// <summary>
        /// Test for checking that adding a Room is working properly
        /// </summary>
        [Fact]
        public async void TestAddRoom()
        {
            var options = new DbContextOptionsBuilder<MySQLDBContext>()
                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            //need to use GUID for unit testing. EnsureDeleted() wasn't working for InMemoryDatabase

            // Set up a context (connection to the "DB") for writing
            using (var context = new MySQLDBContext(options))
            {
                // 2. Act 
                var controller = new RoomsController(context);
                context.Hotel.Add(new Hotel { name = "TEST", phone = "1234567890" });
                await context.SaveChangesAsync();
                Rooms room1 = new Rooms { idHotel = 1, maxCapacity = 2, phoneExtension = 101, rate = 899.99f, roomNumber = "101" };
                var resultActionResult = await controller.AddRoom(room1);
                var result = resultActionResult as ObjectResult;

                // 3. Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);
                Assert.IsType<Rooms>(result.Value);

            }

            // Set up a context (connection to the "DB") for writing
            using (var context = new MySQLDBContext(options))
            {
                // 2. Act 
                var controller = new RoomsController(context);
                var resultActionResult = controller.GetAll();
                var result = resultActionResult as ObjectResult;

                // 3. Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);
                Assert.IsType<List<Rooms>>(result.Value);
                Assert.NotEmpty((System.Collections.IEnumerable)result.Value);
                Assert.Single((System.Collections.IEnumerable)result.Value);
            }

        }

        /// <summary>
        /// Test for checking if getting all the rooms in the database is working properly
        /// </summary>
        [Fact]
        public async void TestGetAll()
        {
            var options = new DbContextOptionsBuilder<MySQLDBContext>()
                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new MySQLDBContext(options))
            {
                // 2. Act 
                var controller = new RoomsController(context);
                context.Hotel.Add(new Hotel { name = "TEST", phone = "1234567890" });
                await context.SaveChangesAsync();
                Rooms room1 = new Rooms { idHotel = 1, maxCapacity = 2, phoneExtension = 101, rate = 899.99f, roomNumber = "101" };
                Rooms room2 = new Rooms { idHotel = 1, maxCapacity = 4, phoneExtension = 102, rate = 799.99f, roomNumber = "102" };
                await controller.AddRoom(room1);
                await controller.AddRoom(room2);
            }

            // Set up a context (connection to the "DB") for writing
            using (var context = new MySQLDBContext(options))
            {
                // 2. Act 
                var controller = new RoomsController(context);
                var resultActionResult = controller.GetAll();
                var result = resultActionResult as OkObjectResult;

                // 3. Assert
                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);
                Assert.IsType<List<Rooms>>(result.Value);
                Assert.NotEmpty((System.Collections.IEnumerable)result.Value);
                Assert.Equal(2, ((List<Rooms>)result.Value).Count);

                context.Database.EnsureDeleted();
            }
        }
    }
}
