using System;
using System.Collections.Generic;
using System.Linq;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Tests.Mocks
{
    internal class InMemoryUserProfileRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserProfileRepository(List<UserProfile> startingData)
        {
            _data = startingData;
        }

        public List<UserProfile> GetAll()
        {
            return _data;
        }

        public UserProfile GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }
        public void Add(UserProfile profile)
        {
            var lastProfile = _data.Last();
            profile.Id = lastProfile.Id + 1;
            _data.Add(profile);
        }
        public void Update(UserProfile profile)
        {
            var currentProfile = _data.FirstOrDefault(p => p.Id == profile.Id);
            if (currentProfile == null)
            {
                return;
            }

            currentProfile.UserName = profile.UserName;
        }

        public void Delete(int id)
        {
            var profileToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (profileToDelete == null)
            {
                return;
            }

            _data.Remove(profileToDelete);
        }
    }
}
