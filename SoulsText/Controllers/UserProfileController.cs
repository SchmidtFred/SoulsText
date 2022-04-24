using Microsoft.AspNetCore.Mvc;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _profileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _profileRepository = userProfileRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(_profileRepository.GetAll());
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var profile = _profileRepository.GetById(id);
            if (profile == null)
            {
                return Json(null);
            }
            return Json(profile);
        }

        [HttpPost]
        public JsonResult Post(UserProfile profile)
        {
            _profileRepository.Add(profile);
            return Json(profile);
        }

        [HttpPut]
        public JsonResult Put(UserProfile profile)
        {
            _profileRepository.Update(profile);
            return Json(profile);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _profileRepository.Delete(id);
            return Json(null);
        }
    }
}
