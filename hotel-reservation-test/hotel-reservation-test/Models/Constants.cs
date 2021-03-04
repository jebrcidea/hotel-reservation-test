using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models
{
    public static class Constants
    {
        public const string roomNumberValueError = "The provided value for roomNumber is invalid";
        public const string maxCapacityError = "The provided value for maxCapacity is invalid";
        public const string rateError = "The provided value for rateError is invalid";
        public const string phoneExtensionValueError = "The provided value for phoneExtension is invalid";
        public const string idHotelDoesntExistError = "The idHotel you're trying to use doesn't exist";
        public const string roomAlreadyExistError = "The room you're trying to create already exists";
        public const string roomExtensionExistError = "The roomExtension you're trying to create already exists";
    }
}
