using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models.Database
{
    public class Configurations
    {
        public int id { get; set; }
        public Int16 maxStayDays { get; set; }
        public Int16 maxReserveAdvanceDays { get; set; }
    }
}
