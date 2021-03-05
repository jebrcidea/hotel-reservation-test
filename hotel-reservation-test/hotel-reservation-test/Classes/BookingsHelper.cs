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
        public static DateTime[] formatDates(DateTime startTime, DateTime endTime)
        {
            DateTime[] response = new DateTime[2];
            DateTime start = new DateTime(startTime.Year, startTime.Month, startTime.Day);
            DateTime end = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
            response[0] = start;
            response[1] = end;
            return response;
        }

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

        public static string validateDateCoherence(DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                return Constants.dateCoheranceError;
            }

            return null;
        }

        //Todo Validate idRoom exists
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
                if ((endTime - startTime).TotalDays > 3)
                    errors.Add(Constants.datesLengthError);
                return errors;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //ToDo get available dates of room
        public static List<RoomAvailability> GetRoomAvailability(int idRoom, DateTime startTime
                                                                ,DateTime endTime, MySQLDBContext mySQLDBContext)
        {
            List<RoomAvailability> response = new List<RoomAvailability>();
            try
            {
                List<Bookings> bookings = mySQLDBContext.Bookings.Where(x => x.idRoom == idRoom &&
                                                                        ((x.startDate >= startTime && x.startDate <= endTime)
                                                                        || (x.endDate >= startTime && x.endDate <= endTime))
                                                                        ).ToList();
                if(bookings.Count == 0)
                {
                    response.Add(new RoomAvailability { startDate = startTime, endDate = endTime });
                    return response;
                }

                DateTime startDate = startTime;
                foreach (Bookings booking in bookings)
                {
                    if(booking.startDate > startDate)
                    {
                        response.Add(new RoomAvailability { startDate = startDate, endDate = booking.startDate.AddSeconds(-1) });
                    }
                    startDate = booking.endDate.AddSeconds(1);
                }

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
