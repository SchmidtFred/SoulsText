using System;
using System.Collections.Generic;
using System.Linq;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Tests.Mocks
{
    internal class InMemoryVoteRepository : IVoteRepository
    {
        private readonly List<Vote> _data;

        public List<Vote> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryVoteRepository(List<Vote> startingData)
        {
            _data = startingData;
        }

        public List<Vote> GetAll()
        {
            return _data;
        }

        public Vote GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Vote vote)
        {
            var lastVote = _data.Last();
            vote.Id = lastVote.Id + 1;
            _data.Add(vote);
        }
        public void Update(Vote vote)
        {
            var currentVote = _data.FirstOrDefault(p => p.Id == vote.Id);
            if (currentVote == null)
            {
                return;
            }

            currentVote.Upvote = vote.Upvote;
            currentVote.UserProfileId = vote.UserProfileId;
            currentVote.MessageId = vote.MessageId;
        }

        public void Delete(int id)
        {
            var voteToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (voteToDelete == null)
            {
                return;
            }

            _data.Remove(voteToDelete);
        }
    }
}
