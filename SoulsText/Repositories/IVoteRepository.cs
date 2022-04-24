using SoulsText.Models;
using System.Collections.Generic;

namespace SoulsText.Repositories
{
    public interface IVoteRepository
    {
        void Add(Vote vote);
        void Delete(int id);
        List<Vote> GetAll();
        Vote GetById(int id);
        void Update(Vote vote);
    }
}