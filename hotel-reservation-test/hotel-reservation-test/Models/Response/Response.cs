using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Models.Response
{
    public class Response
    {
        public int code { get; set; }
        public string message { get; set; }
        public object extra { get; set; }
    }
}
