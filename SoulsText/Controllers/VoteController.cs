using Microsoft.AspNetCore.Mvc;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : Controller
    {
        private readonly IVoteRepository _voteRepository;

        public VoteController(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }
        
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_voteRepository.GetAll());
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var vote = _voteRepository.GetById(id);
            if (vote == null)
            {
                return Json(null);
            }
            return Json(vote);
        }

        [HttpPost]
        public JsonResult Post(Vote vote)
        {
            _voteRepository.Add(vote);
            return Json(vote);
        }

        [HttpPut]
        public JsonResult Put(Vote vote)
        {
            _voteRepository.Update(vote);
            return Json(vote);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _voteRepository.Delete(id);
            return Json(null);
        }
    }
}
