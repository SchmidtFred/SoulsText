using System;
using System.Collections.Generic;
using System.Linq;
using SoulsText.Models;
using SoulsText.Repositories;

namespace SoulsText.Tests.Mocks
{
    internal class InMemoryMessageRepository : IMessageRepository
    {
        private readonly List<Message> _data;

        public List<Message> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryMessageRepository(List<Message> startingData)
        {
            _data = startingData;
        }

        public List<Message> GetAll()
        {
            return _data;
        }

        public Message GetById(int id)
        {
            return _data.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Message message)
        {
            var lastMessage = _data.Last();
            message.Id = lastMessage.Id + 1;
            _data.Add(message);
        }
        public void Update(Message message)
        {
            var currentMessage = _data.FirstOrDefault(p => p.Id == message.Id);
            if (currentMessage == null)
            {
                return;
            }

            currentMessage.Content = message.Content;
            currentMessage.X = message.X;
            currentMessage.Y = message.Y;
            currentMessage.Z = message.Z;
            currentMessage.UserProfileId = message.UserProfileId;
        }

        public void Delete(int id)
        {
            var messageToDelete = _data.FirstOrDefault(p => p.Id == id);
            if (messageToDelete == null)
            {
                return;
            }

            _data.Remove(messageToDelete);
        }
    }
}
