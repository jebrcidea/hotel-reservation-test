using hotel_reservation_test.DBContexts;
using hotel_reservation_test.Models;
using hotel_reservation_test.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_reservation_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private MySQLDBContext mySQLDBContext;

        public TestController(MySQLDBContext context)
        {
            mySQLDBContext = context;
        }

        [HttpGet]
        public IList<Test2> Get()
        {
            var query = this.mySQLDBContext.Test.Join(
                    mySQLDBContext.Test,
                    Test => Test.id,
                    Test2 => Test2.id,
                    (Test, Test2) => new Test2
                    {
                        id = Test.id,
                        testoru = Test.testoru,
                        id2 = Test2.id+1,
                        testoru2 = Test2.testoru
                    }
                ).ToList();
            return query;
            //return (this.mySQLDBContext.Test.Where(x => x.id < 6).ToList());
        }

        [HttpPost]
        public async Task<Test> PostAsync(Test newRecord)
        {
            try
            {
                this.mySQLDBContext.Test.Add(newRecord);
                Task saveTask = this.mySQLDBContext.SaveChangesAsync();
                //wait until it finishes to make sure no exception was thrown
                await saveTask;
            }
            catch(Exception e)
            {
                newRecord.id = -1;
                newRecord.testoru = e.Message;
            }
            return newRecord;

        }
    }
}
