using hotel_reservation_test.DBContexts;
using hotel_reservation_test.Models;
using hotel_reservation_test.Models.Database;
using hotel_reservation_test.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Classes
{
    public static class BookingsHelper
    {
        /// <summary>
        /// Formats the date so the first one starts at the begining of the day
        /// and the last one ends at the end of that day
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>an array of 2 dates start at 0 and end at 1</returns>
        public static DateTime[] formatDates(DateTime startTime, DateTime endTime)
        {
            DateTime[] response = new DateTime[2];
            DateTime start = new DateTime(startTime.Year, startTime.Month, startTime.Day);
            DateTime end = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
            response[0] = start;
            response[1] = end;
            return response;
        }

        /// <summary>
        /// Validates that the starts at least at the next full day and it's no more than 
        /// maxReserveAdvanceDays days into the future
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A string with the error or null.</returns>
        public static string dateValidation(DateTime date)
        {
            DateTime tomorrow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
            if(date < tomorrow)
            {
                return Constants.startDateBeforeError;
            }

            DateTime oneMonthFuture = tomorrow.AddDays(Constants.maxReserveAdvanceDays);
            if (date > oneMonthFuture)
            {
                return Constants.startDateAfterError;
            }
            return null;
        }

        /// <summary>
        /// Checks that endTime is not smaller that startTime
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>a string with the error or null</returns>
        public static string validateDateCoherence(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                return Constants.dateCoheranceError;
            }

            return null;
        }

        /// <summary>
        /// Validates the logic of the dates according to business rules.
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="mySQLDBContext"></param>
        /// <param name="idRoom"></param>
        /// <returns>A list of strings with the errors</returns>
        public static async Task<List<string>> validations(DateTime startTime, DateTime endTime
                                                ,MySQLDBContext mySQLDBContext, int idRoom)
        {
            try
            {
                List<String> response = new List<string>();
                string tempString = await RoomsHelper.validateRoomExists(idRoom, mySQLDBContext);
                if (!String.IsNullOrEmpty(tempString)) response.Add(tempString);
                tempString = dateValidation(startTime);
                if (!String.IsNullOrEmpty(tempString)) response.Add(tempString);
                tempString = dateValidation(endTime);
                if (!String.IsNullOrEmpty(tempString)) response.Add(tempString);
                tempString = validateDateCoherence(startTime, endTime);
                if (!String.IsNullOrEmpty(tempString)) response.Add(tempString);
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Checks that the desired room is available and follow the business rule of the max stay
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="mySQLDBContext"></param>
        /// <param name="idRoom"></param>
        /// <param name="isInsert"></param>
        /// <param name="bookingID"></param>
        /// <returns>Returns a list of errors</returns>
        public static List<string> validationsBooking(DateTime startTime, DateTime endTime
                                                , MySQLDBContext mySQLDBContext, int idRoom
                                                , Boolean isInsert = true, int bookingID = 0)
        {
            List<string> errors = new List<string>();
            try
            {
                Boolean roomAvailable = isDateAvailable(idRoom, startTime, endTime, mySQLDBContext, isInsert, bookingID);
                if (!roomAvailable)
                    errors.Add(Constants.roomBookedError);
                if ((endTime - startTime).TotalDays > Constants.maxStayDays)
                    errors.Add(Constants.datesLengthError);
                return errors;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets a list of date ranges where the room is available
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="mySQLDBContext"></param>
        /// <returns>A list of start and end dates for the availability of that room</returns>
        public static List<RoomAvailability> GetRoomAvailability(int idRoom, DateTime startTime
                                                                ,DateTime endTime, MySQLDBContext mySQLDBContext)
        {
            List<RoomAvailability> response = new List<RoomAvailability>();
            try
            {
                //gets all the bookings on the room and dates they're asking
                List<Bookings> bookings = mySQLDBContext.Bookings.Where(x => x.idRoom == idRoom &&
                                                                        ((x.startDate >= startTime && x.startDate <= endTime)
                                                                        || (x.endDate >= startTime && x.endDate <= endTime))
                                                                        ).ToList();
                //if there's no bookings that means it's all available so we just build that response and return
                if(bookings.Count == 0)
                {
                    response.Add(new RoomAvailability { startDate = startTime, endDate = endTime });
                    return response;
                }

                DateTime startDate = startTime;
                foreach (Bookings booking in bookings)
                {
                    //if we have a time gap before the first booking on the interval 
                    //we add that to our response list
                    if(booking.startDate > startDate)
                    {
                        response.Add(new RoomAvailability { startDate = startDate, endDate = booking.startDate.AddSeconds(-1) });
                    }
                    //we set the next block startDate to after the current booking ends
                    startDate = booking.endDate.AddSeconds(1);
                }

                //in case we had a gap at the end, we add it at the end
                if(startDate < endTime)
                {
                    response.Add(new RoomAvailability { startDate = startDate, endDate = endTime });
                }

                return response;

            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Checks if there's another booking that has conflict with the one we're trying to create.
        /// If it's for an update it ignores this booking since it will change.
        /// </summary>
        /// <param name="idRoom"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="mySQLDBContext"></param>
        /// <param name="isInsert"></param>
        /// <param name="idBooking"></param>
        /// <returns>A boolean to if the date was available or not</returns>
        public static Boolean isDateAvailable(int idRoom, DateTime startTime
                                               , DateTime endTime, MySQLDBContext mySQLDBContext
                                                , Boolean isInsert = true, int idBooking = 0)
        {
            try
            {
                if (isInsert)
                {
                    int exists = mySQLDBContext.Bookings.Where(x => x.idRoom == idRoom &&
                                                                        ((x.startDate >= startTime && x.startDate <= endTime)
                                                                        || (x.endDate >= startTime && x.endDate <= endTime))
                                                                        ).Count();

                    if (exists == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    int exists = mySQLDBContext.Bookings.Where(x => x.idRoom == idRoom &&
                                                                            ((x.startDate >= startTime && x.startDate <= endTime)
                                                                            || (x.endDate >= startTime && x.endDate <= endTime))
                                                                            && x.id != idBooking
                                                                            ).Count();

                    if (exists == 0)
                    {
                        return true;
                    }
                }


                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
