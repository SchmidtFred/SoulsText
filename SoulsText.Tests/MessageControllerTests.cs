using SoulsText.Models;
using SoulsText.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SoulsText.Controllers;

namespace Streamish.Tests
{
    public class MessageControllerTests
    {
        [Fact]
        public void Get_Returns_All_Messages()
        {
            // Arrange 
            var messageCount = 20;
            var messages = CreateTestMessages(messageCount);

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            // Act 
            var result = controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actualVideos = Assert.IsType<List<Message>>(jsonResult.Value);

            Assert.Equal(messageCount, actualVideos.Count);
            Assert.Equal(messages, actualVideos);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var messages = new List<Message>();

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public void Get_By_Id_Returns_Message_With_Given_Id()
        {
            // Arrange
            var testMessageId = 99;
            var messages = CreateTestMessages(5);
            messages[0].Id = testMessageId;

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            // Act
            var result = controller.Get(testMessageId);

            // Assert
            var JsonResult = Assert.IsType<JsonResult>(result);
            var actualMessage = Assert.IsType<Message>(JsonResult.Value);

            Assert.Equal(testMessageId, actualMessage.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_Message()
        {
            // Arrange 
            var messageCount = 20;
            var messages = CreateTestMessages(messageCount);

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            // Act
            var newMessage = new Message()
            {
                Content = "Content",
                X = 1,
                Y = 2,
                Z = 3,
                UserProfileId = 1
            };

            controller.Post(newMessage);

            // Assert
            Assert.Equal(messageCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Updates_A_Message()
        {
            // Arrange
            var testMessageId = 99;
            var messages = CreateTestMessages(5);
            messages[0].Id = testMessageId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            var messageToUpdate = new Message()
            {
                Id = testMessageId,
                Content = "Updated!",
                X = 1,
                Y = 2,
                Z = 3,
                UserProfileId = 1
            };

            // Act
            controller.Put(messageToUpdate);

            // Assert
            var messageFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testMessageId);
            Assert.NotNull(messageFromDb);

            Assert.Equal(messageToUpdate.Content, messageFromDb.Content);
            Assert.Equal(messageToUpdate.X, messageFromDb.X);
            Assert.Equal(messageToUpdate.Y, messageFromDb.Y);
            Assert.Equal(messageToUpdate.Z, messageFromDb.Z);
            Assert.Equal(messageToUpdate.UserProfileId, messageFromDb.UserProfileId);
        }

        [Fact]
        public void Delete_Method_Removes_A_Message()
        {
            // Arrange
            var testMessageId = 99;
            var messages = CreateTestMessages(5);
            messages[0].Id = testMessageId;

            var repo = new InMemoryMessageRepository(messages);
            var controller = new MessageController(repo);

            // Act
            controller.Delete(testMessageId);

            // Assert
            var messageFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testMessageId);
            Assert.Null(messageFromDb);
        }

        private List<Message> CreateTestMessages(int count)
        {
            var messages = new List<Message>();
            for (var i = 1; i <= count; i++)
            {
                messages.Add(CreateTestMessage(i));
            }
            return messages;
        }

        private Message CreateTestMessage(int id)
        {
            return new Message()
            {
                Id = id,
                Content = "Content",
                X = 0,
                Y = 0,
                Z = 0,
                UserProfileId = 1
            };
        }
    }
}