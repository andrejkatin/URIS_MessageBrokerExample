using MessageBrokerBus.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocketHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBroker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery] string topic, [FromQuery] string group)
        {
            if (topic == null || group == null)
            {
                return StatusCode(400, "Invalid parameters of request");
            }

            IMessageService MessageService = SocketManager.MessageService;

            List<string> messages = MessageService.Consume(topic, group);

            Console.WriteLine("Messages: " + messages + " consumed from API");

            return new JsonResult(messages);
        }

        // POST 
        [HttpPost]
        public ActionResult Post([FromBody] object incomingMessage, [FromQuery] string topic)
        {
            IMessageService MessageService = SocketManager.MessageService;

            try
            {
                MessageService.Publish(incomingMessage.ToString(), topic);
            }
            catch
            {
                Response.StatusCode = 400;
                Console.WriteLine("Message: " + incomingMessage + " rejected. Bad message format.");
                return Content("Bad message format");
            }

            return Ok(incomingMessage);
        }
    }
}
