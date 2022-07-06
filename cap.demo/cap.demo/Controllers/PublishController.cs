using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;
using cap.demo.MySql;

namespace cap.demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly ICapPublisher _capBus;

        public PublishController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        [Route("~/send")]
        [HttpGet]
        public IActionResult SendMessage([FromServices] ICapPublisher capBus)
        {
            var header = new Dictionary<string, string>()
            {
                ["my.header.first"] = "first",
                ["my.header.second"] = "second"
            };

            capBus.Publish("test.show.time", DateTime.Now, header);
            //capBus.Publish("test.show.time", DateTime.Now);

            return Ok();
        }

        [Route("~/ef/transaction")]
        [HttpGet]
        public IActionResult EntityFrameworkWithTransaction([FromServices] AppDbContext dbContext)
        {
            using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                dbContext.Persons.Add(new Person() { Name = "ef.transaction" });

                for (int i = 0; i < 1; i++)
                {
                    _capBus.Publish("sample.rabbitmq.mysql", DateTime.Now);
                }

                dbContext.SaveChanges();

                trans.Commit();
            }
            return Ok();
        }
    }
}
