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
    public class VoteControllerTests
    {
        [Fact]
        public void Get_Returns_All_Votes()
        {
            // Arrange 
            var voteCount = 20;
            var votes = CreateTestVotes(voteCount);

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            // Act 
            var result = controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actualVideos = Assert.IsType<List<Vote>>(jsonResult.Value);

            Assert.Equal(voteCount, actualVideos.Count);
            Assert.Equal(votes, actualVideos);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var votes = new List<Vote>();

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public void Get_By_Id_Returns_Vote_With_Given_Id()
        {
            // Arrange
            var testVoteId = 99;
            var votes = CreateTestVotes(5);
            votes[0].Id = testVoteId;

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            // Act
            var result = controller.Get(testVoteId);

            // Assert
            var JsonResult = Assert.IsType<JsonResult>(result);
            var actualVote = Assert.IsType<Vote>(JsonResult.Value);

            Assert.Equal(testVoteId, actualVote.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_Vote()
        {
            // Arrange 
            var voteCount = 20;
            var votes = CreateTestVotes(voteCount);

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            // Act
            var newVote = new Vote()
            {
                Upvote = false,
                UserProfileId = 1,
                MessageId = 1
            };

            controller.Post(newVote);

            // Assert
            Assert.Equal(voteCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Updates_A_Vote()
        {
            // Arrange
            var testVoteId = 99;
            var votes = CreateTestVotes(5);
            votes[0].Id = testVoteId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            var voteToUpdate = new Vote()
            {
                Id = testVoteId,
                Upvote = true,
                UserProfileId = 2,
                MessageId = 2
            };

            // Act
            controller.Put(voteToUpdate);

            // Assert
            var voteFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testVoteId);
            Assert.NotNull(voteFromDb);

            Assert.Equal(voteToUpdate.Id, voteFromDb.Id);
            Assert.Equal(voteToUpdate.Upvote, voteFromDb.Upvote);
            Assert.Equal(voteToUpdate.UserProfileId, voteFromDb.UserProfileId);
            Assert.Equal(voteToUpdate.MessageId, voteFromDb.MessageId);
        }

        [Fact]
        public void Delete_Method_Removes_A_Vote()
        {
            // Arrange
            var testVoteId = 99;
            var votes = CreateTestVotes(5);
            votes[0].Id = testVoteId;

            var repo = new InMemoryVoteRepository(votes);
            var controller = new VoteController(repo);

            // Act
            controller.Delete(testVoteId);

            // Assert
            var voteFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testVoteId);
            Assert.Null(voteFromDb);
        }

        private List<Vote> CreateTestVotes(int count)
        {
            var votes = new List<Vote>();
            for (var i = 1; i <= count; i++)
            {
                votes.Add(CreateTestVote(i));
            }
            return votes;
        }

        private Vote CreateTestVote(int id)
        {
            return new Vote()
            {
                Id = id,
                Upvote = false,
                UserProfileId = 1,
                MessageId = 1
            };
        }
    }
}