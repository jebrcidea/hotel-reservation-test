using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models.Input
{
    public class NewBooking
    {
        public int idRoom { get; set; }
        public DateTime startDate { get;set;}
        public DateTime endDate { get;set;}
    }
}
