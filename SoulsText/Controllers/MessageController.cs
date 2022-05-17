using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SoulsText.Hubs;
using SoulsText.Models;
using SoulsText.Repositories;
using System.Threading.Tasks;

namespace SoulsText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<SoulsHub> _hubContext;
        //the logger here is more for testing purposes as I learn how to use logging. Will do full implementation in the Hubs.
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageRepository messageRepository, IHubContext<SoulsHub> hubContext, ILogger<MessageController> logger)
        {
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Get()
        {
            _logger.LogInformation("Getting all messages...");

            return Json(_messageRepository.GetAll());
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
            {
                return Json(null);
            }
            return Json(message);
        }

        //[HttpPost]
        //public JsonResult Post(Message message)
        //{
        //    _messageRepository.Add(message);
        //    return Json(message);
        //}

        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Message message)
        {
            _messageRepository.Add(message);
            await _hubContext.Clients.All.SendAsync("ReceiveNewMessage", message);
            return Json(message);
        }

        [HttpPut]
        public JsonResult Put(Message message)
        {
            _messageRepository.Update(message);
            return Json(message);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _messageRepository.Delete(id);
            return Json(null);
        }
    }
}
