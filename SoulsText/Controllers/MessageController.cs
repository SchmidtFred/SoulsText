using Microsoft.AspNetCore.Mvc;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
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

        [HttpPost]
        public JsonResult Post(Message message)
        {
            _messageRepository.Add(message);
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
