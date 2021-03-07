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
        /// <summary>
        /// Validates the value of all the variables of the Rooms Model makes logical sense
        /// </summary>
        /// <param name="room"></param>
        /// <param name="isInsert"></param>
        /// <returns>A list of errors</returns>
        public static List<string> StaticValidations(Rooms room, Boolean isInsert = true)
        {
            List<string> errors = new List<string>();
            //only validates the room id if it's not insert since this values is assign by the DB
            if (!isInsert)
            {
                if(room.id < 1)
                {
                    errors.Add(Constants.roomIdValueError);
                }
            }

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

        /// <summary>
        /// Validates all the variables from Rooms that need to be unique on the DB 
        /// or that require values from another table
        /// </summary>
        /// <param name="room"></param>
        /// <param name="mySQLDBContext"></param>
        /// <param name="isInsert"></param>
        /// <returns></returns>
        public static async Task<List<string>> DatabaseValidations(Rooms room, MySQLDBContext mySQLDBContext, Boolean isInsert = true)
        {
            List<string> errors = new List<string>();
            try
            {
                //if it's insert it checks the unique values don't already exist on the DB
                if (isInsert)
                {
                    //Check if idHotel exists
                    int hotelCount = await mySQLDBContext.Hotel.Where(x => x.id == room.idHotel).CountAsync();

                    //Check if combination idHotel and roomNumber exists
                    int hotelRoomCount = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                            && x.roomNumber == room.roomNumber)
                                                                        .CountAsync();

                    //ToDo check PhoneExtension not on database
                    int hotelPhoneExtensionCount = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                            && x.phoneExtension == room.phoneExtension)
                                                                        .CountAsync();
                    if (hotelCount != 1)
                    {
                        errors.Add(Constants.idHotelDoesntExistError);
                    }

                    if (hotelRoomCount != 0)
                    {
                        errors.Add(Constants.roomAlreadyExistError);
                    }

                    if (hotelPhoneExtensionCount != 0)
                    {
                        errors.Add(Constants.roomExtensionExistError);
                    }

                    return errors;
                }

                //if it's an update it checks that the value don't exist previously for a record different
                //to the one that's been modified
                int hotelRoomCount2 = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                        && x.roomNumber == room.roomNumber
                                                                        && x.id != room.id)
                                                                    .CountAsync();

                //ToDo check PhoneExtension not on database
                int hotelPhoneExtensionCount2 = await mySQLDBContext.Rooms.Where(x => x.idHotel == room.idHotel
                                                                        && x.phoneExtension == room.phoneExtension
                                                                        && x.id != room.id)
                                                                    .CountAsync();

                if (hotelRoomCount2 != 0)
                {
                    errors.Add(Constants.roomAlreadyExistError);
                }

                if (hotelPhoneExtensionCount2 != 0)
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

        /// <summary>
        /// Validate the variables of rooms both the logic and the ones that need coherence with the database
        /// </summary>
        /// <param name="room"></param>
        /// <param name="mySQLDBContext"></param>
        /// <param name="isInsert"></param>
        /// <returns>List of errors</returns>
        public static async Task<List<string>> Validations(Rooms room, MySQLDBContext mySQLDBContext, Boolean isInsert = true)
        {
            List<string> errors = new List<string>();
            try
            {
                errors.AddRange(StaticValidations(room, isInsert));
                errors.AddRange(await DatabaseValidations(room, mySQLDBContext, isInsert));
            }
            catch(Exception e)
            {
                throw e;
            }
            
            return errors;
        }

        /// <summary>
        /// Checks if the room exists on the DB
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="mySQLDBContext"></param>
        /// <returns>An error string or null</returns>
        public static async Task<string> validateRoomExists(int idRoom, MySQLDBContext mySQLDBContext)
        {
            try
            {
                int roomCount = await mySQLDBContext.Rooms.Where(x => x.id == idRoom).CountAsync();
                if(roomCount != 1)
                {
                    return Constants.roomIdValueError;
                }
                return null;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
