using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models.Database
{
    public class Rooms
    {
        public int id { get;set; }
        public int idHotel { get; set; }
        public string roomNumber { get; set; }
        public int maxCapacity { get; set; }
        public float rate { get; set; }
        public int phoneExtension { get; set; }
    }
}
