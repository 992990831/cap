using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace cap.demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        [NonAction]
        [CapSubscribe("test.show.time")]
        public void ReceiveMessage(DateTime time, [FromCap] CapHeader header)
        {
            Console.WriteLine("message time is:" + time);
            Console.WriteLine("message firset header :" + header["my.header.first"]);
            Console.WriteLine("message second header :" + header["my.header.second"]);
        }
    }
}
