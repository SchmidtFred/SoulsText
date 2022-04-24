using SoulsText.Models;
using System.Collections.Generic;

namespace SoulsText.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile profile);
        void Delete(int id);
        List<UserProfile> GetAll();
        UserProfile GetById(int id);
        void Update(UserProfile profile);
    }
}