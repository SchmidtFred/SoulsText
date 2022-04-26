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
    public class UserProfileControllerTests
    {
        [Fact]
        public void Get_Returns_All_Profiles()
        {
            // Arrange 
            var profileCount = 20;
            var profiles = CreateTestProfiles(profileCount);

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            // Act 
            var result = controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actualVideos = Assert.IsType<List<UserProfile>>(jsonResult.Value);

            Assert.Equal(profileCount, actualVideos.Count);
            Assert.Equal(profiles, actualVideos);
        }

        [Fact]
        public void Get_By_Id_Returns_NotFound_When_Given_Unknown_id()
        {
            // Arrange 
            var profiles = new List<UserProfile>();

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            // Act
            var result = controller.Get(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public void Get_By_Id_Returns_Profile_With_Given_Id()
        {
            // Arrange
            var testProfileId = 99;
            var profiles = CreateTestProfiles(5);
            profiles[0].Id = testProfileId;

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            // Act
            var result = controller.Get(testProfileId);

            // Assert
            var JsonResult = Assert.IsType<JsonResult>(result);
            var actualProfile = Assert.IsType<UserProfile>(JsonResult.Value);

            Assert.Equal(testProfileId, actualProfile.Id);
        }

        [Fact]
        public void Post_Method_Adds_A_New_Profile()
        {
            // Arrange 
            var profileCount = 20;
            var profiles = CreateTestProfiles(profileCount);

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            // Act
            var newProfile = new UserProfile()
            {
                UserName = "UserName",
            };

            controller.Post(newProfile);

            // Assert
            Assert.Equal(profileCount + 1, repo.InternalData.Count);
        }

        [Fact]
        public void Put_Method_Updates_A_Profile()
        {
            // Arrange
            var testProfileId = 99;
            var profiles = CreateTestProfiles(5);
            profiles[0].Id = testProfileId; // Make sure we know the Id of one of the videos

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            var profileToUpdate = new UserProfile()
            {
                Id = testProfileId,
                UserName = "UPDATED!"
            };

            // Act
            controller.Put(profileToUpdate);

            // Assert
            var profileFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testProfileId);
            Assert.NotNull(profileFromDb);

            Assert.Equal(profileToUpdate.UserName, profileFromDb.UserName);
        }

        [Fact]
        public void Delete_Method_Removes_A_Profile()
        {
            // Arrange
            var testProfileId = 99;
            var profiles = CreateTestProfiles(5);
            profiles[0].Id = testProfileId;

            var repo = new InMemoryUserProfileRepository(profiles);
            var controller = new UserProfileController(repo);

            // Act
            controller.Delete(testProfileId);

            // Assert
            var profileFromDb = repo.InternalData.FirstOrDefault(p => p.Id == testProfileId);
            Assert.Null(profileFromDb);
        }

        private List<UserProfile> CreateTestProfiles(int count)
        {
            var profiles = new List<UserProfile>();
            for (var i = 1; i <= count; i++)
            {
                profiles.Add(CreateTestUserProfile(i));
            }
            return profiles;
        }

        private UserProfile CreateTestUserProfile(int id)
        {
            return new UserProfile()
            {
                Id = id,
                UserName = $"User {id}"
            };
        }
    }
}