using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models.Database
{
    public class Bookings
    {
        public int id { get; set; }
        public int idRoom { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
    }
}
