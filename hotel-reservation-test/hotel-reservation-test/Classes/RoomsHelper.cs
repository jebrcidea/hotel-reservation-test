using hotel_reservation_test.DBContexts;
using hotel_reservation_test.Models;
using hotel_reservation_test.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace hotel_reservation_test.Classes
{
    public static class RoomsHelper
    {
        public static List<string> StaticValidations(Rooms room)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(room.roomNumber))
            {
                errors.Add(Constants.roomNumberValueError);
            }

            if (room.maxCapacity < 1)
            {
                errors.Add(Constants.maxCapacityError);
            }

            if (room.rate < 0)
            {
                errors.Add(Constants.rateError);
            }

            if(room.phoneExtension < 1)
            {
                errors.Add(Constants.phoneExtensionValueError);
            }

            return errors;

        }

        public static async Task<List<string>> DatabaseValidations(Rooms room, MySQLDBContext mySQLDBContext, Boolean isInsert = true)
        {
            List<string> errors = new List<string>();
            try
            {
                if (isInsert)
                {
                    //Check if idHotel exists
                    int hotelCount = await mySQLDBContext.Hotel.Where(x => x.id == room.idHotel).CountAsync();

                    //Check if combination idHotel and roomNumber exists
                    int hotelRoomCount = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                            && x.roomNumber == room.roomNumber)
                                                                        .CountAsync();
                    if (hotelCount != 1)
                    {
                        errors.Add(Constants.idHotelDoesntExistError);
                    }

                    if (hotelRoomCount != 0)
                    {
                        errors.Add(Constants.roomAlreadyExistError);
                    }
                }
                else
                {
                    int hotelRoomCount = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                            && x.roomNumber == room.roomNumber
                                                                            && x.id != room.id)
                                                                        .CountAsync();

                    if (hotelRoomCount != 0)
                    {
                        errors.Add(Constants.roomAlreadyExistError);
                    }
                }


                //ToDo check PhoneExtension not on database
                int hotelPhoneExtensionCount = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                        && x.phoneExtension == room.phoneExtension)
                                                                    .CountAsync();
                

                if (hotelPhoneExtensionCount != 0)
                {
                    errors.Add(Constants.roomExtensionExistError);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return errors;
        }

        public static async Task<List<string>> Validations(Rooms room, MySQLDBContext mySQLDBContext, Boolean isInsert = true)
        {
            List<string> errors = new List<string>();
            try
            {
                errors.AddRange(StaticValidations(room));
                errors.AddRange(await DatabaseValidations(room, mySQLDBContext, isInsert));
            }
            catch(Exception e)
            {
                throw e;
            }
            
            return errors;
        }
    }
}
