using hotel_reservation_test.Classes;
using hotel_reservation_test.DBContexts;
using hotel_reservation_test.Models;
using hotel_reservation_test.Models.Database;
using hotel_reservation_test.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private MySQLDBContext mySQLDBContext;

        public BookingsController(MySQLDBContext context)
        {
            mySQLDBContext = context;
        }

        [HttpGet("{idBooking}")]
        public IActionResult GetOne(int idBooking)
        {
            try
            {
                Bookings booking = this.mySQLDBContext.Bookings.Where(x => x.id == idBooking).FirstOrDefault();
                if (booking != null)
                {
                    return Ok(booking);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("{idRoom}/availability")]
        public async Task<IActionResult> GetAvailabilityRoom(int idRoom, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                DateTime[] formatedDates = BookingsHelper.formatDates(startDate, endDate);
                startDate = formatedDates[0];
                endDate = formatedDates[1];

                List<string> errors = await BookingsHelper.validations(startDate, endDate, mySQLDBContext, idRoom);
                if(errors.Count != 0)
                {
                    return BadRequest(errors);
                }

                List<RoomAvailability> availability = BookingsHelper.GetRoomAvailability(idRoom, startDate, endDate, mySQLDBContext);
                return Ok(availability);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("{idRoom}/create")]
        public async Task<IActionResult> newBooking(int idRoom, DateTime startDate, DateTime endDate)
        {
            try
            {
                DateTime[] formatedDates = BookingsHelper.formatDates(startDate, endDate);
                startDate = formatedDates[0];
                endDate = formatedDates[1];

                List<string> errors = await BookingsHelper.validations(startDate, endDate, mySQLDBContext, idRoom);
                errors.AddRange(BookingsHelper.validationsBooking(startDate, endDate, mySQLDBContext, idRoom));

                if (errors.Count != 0)
                    return BadRequest(errors);

                Bookings newBooking = new Bookings { idRoom = idRoom, startDate = startDate, endDate = endDate };
                await mySQLDBContext.AddAsync(newBooking);
                await mySQLDBContext.SaveChangesAsync();
                return Ok(newBooking);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("modify")]
        public async Task<IActionResult> modifyBooking(Bookings booking)
        {
            try
            {
                DateTime[] formatedDates = BookingsHelper.formatDates(booking.startDate, booking.endDate);
                booking.startDate = formatedDates[0];
                booking.endDate = formatedDates[1];

                Bookings databaseBooking = await mySQLDBContext.Bookings.FindAsync(booking.id);
                if (databaseBooking == null)
                    return NotFound();

                List<string> errors = await BookingsHelper.validations(booking.startDate, booking.endDate, mySQLDBContext, booking.idRoom);
                errors.AddRange(BookingsHelper.validationsBooking(booking.startDate, booking.endDate, mySQLDBContext, booking.idRoom, false, booking.id));

                if (errors.Count != 0)
                    return BadRequest(errors);

                databaseBooking.startDate = booking.startDate;
                databaseBooking.endDate = booking.endDate;
                await mySQLDBContext.SaveChangesAsync();
                return Ok(databaseBooking);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{idBooking}")]
        public async Task<IActionResult> deleteBooking(int idBooking)
        {
            try
            {
                Bookings booking = this.mySQLDBContext.Bookings.Where(x => x.id == idBooking).FirstOrDefault();
                if (booking != null)
                {
                    mySQLDBContext.Bookings.Remove(booking);
                    await mySQLDBContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

        }
    }
}
