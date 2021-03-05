using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models
{
    public static class Constants
    {
        public const string roomNumberValueError = "The provided value for roomNumber is invalid";
        public const string roomIdValueError = "The provided value for room id is invalid";
        public const string maxCapacityError = "The provided value for maxCapacity is invalid";
        public const string rateError = "The provided value for rate is invalid";
        public const string phoneExtensionValueError = "The provided value for phoneExtension is invalid";
        public const string idHotelDoesntExistError = "The idHotel you're trying to use doesn't exist";
        public const string roomAlreadyExistError = "The room you're trying to create already exists";
        public const string roomExtensionExistError = "The roomExtension you're trying to create already exists";
        public const string startDateBeforeError = "You can only select dates starting the next day";
        public const string startDateAfterError = "You can only select dates at most 30 days in advance";
        public const string datesLengthError = "You can only book a room for a maximum of 3 days";
        public const string dateCoheranceError = "startDate should be smaller than endDate";
        public const string roomBookedError = "The room you selected is not available in the desired dates";
        public const int maxStayDays = 3; //also modify datesLengthError
        public const int maxReserveAdvanceDays = 30; //also modify startDateAfterError
    }
}
