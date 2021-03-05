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
    public class RoomsController : ControllerBase
    {
        private MySQLDBContext mySQLDBContext;

        public RoomsController(MySQLDBContext context)
        {
            mySQLDBContext = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<Rooms> rooms = this.mySQLDBContext.Rooms.ToList();
                return Ok(rooms);
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{idRoom}")]
        public IActionResult GetOne(int idRoom)
        {
            try
            {
                Rooms room = this.mySQLDBContext.Rooms.Where(x => x.id == idRoom).FirstOrDefault();
                if(room != null)
                {
                    return Ok(room);
                }
                return NotFound();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
             
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(Rooms room)
        {
            try
            {
                List<string> errors = await RoomsHelper.Validations(room, mySQLDBContext);
                if (errors.Count != 0)
                {
                    return BadRequest(errors);
                }
                await mySQLDBContext.AddAsync(room);
                await mySQLDBContext.SaveChangesAsync();
                return Ok(room);
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> ModifyRoom(Rooms room)
        {
            try
            {
                List<string> errors = await RoomsHelper.Validations(room, mySQLDBContext, false);
                if (errors.Count != 0)
                {
                    return BadRequest(errors);
                }

                Rooms inDatabaseRoom = mySQLDBContext.Rooms.FirstOrDefault(x => x.id == room.id && x.idHotel == room.idHotel);

                inDatabaseRoom.roomNumber = room.roomNumber;
                inDatabaseRoom.maxCapacity = room.maxCapacity;
                inDatabaseRoom.rate = room.rate;
                inDatabaseRoom.phoneExtension = room.phoneExtension;

                await mySQLDBContext.SaveChangesAsync();
                return Ok(inDatabaseRoom);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{idRoom}")]
        public async Task<IActionResult> DeleteRoom(int idRoom)
        {
            try
            {
                Rooms inDatabaseRoom = mySQLDBContext.Rooms.FirstOrDefault(x => x.id == idRoom );

                if(inDatabaseRoom != null)
                {
                    mySQLDBContext.Remove(inDatabaseRoom);

                    await mySQLDBContext.SaveChangesAsync();
                    return Ok(inDatabaseRoom);
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
