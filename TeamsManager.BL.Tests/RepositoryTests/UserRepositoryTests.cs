using System;
using System.Collections.Generic;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class UserRepositoryTests : IClassFixture<UserRepositoryTestsFixture>, IDisposable
    {
        private readonly UserRepositoryTestsFixture userRepositoryTestsFixture;

        private UserRepository RepositorySUT => userRepositoryTestsFixture.Repository;

        public UserRepositoryTests(UserRepositoryTestsFixture userRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.userRepositoryTestsFixture = userRepositoryTestsFixture;

            this.userRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var userModel = new UserModel
            {
                Id = 10,
                Email = "User1@email.cz",
                FirstName = "Name1",
                LastName = "Surname1",
                Password = "Password1",
                UserDescription = "First user",
                ProfilePicture = new ProfileImageLightModel(),
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>()
            };

            var returnedUser = RepositorySUT.Add(userModel);

            Assert.NotNull(returnedUser);
            RepositorySUT.Remove(returnedUser.Id);
        }

        [Fact]
        public void GetAll_WithExistingUsers_DoesNotThrow()
        {
            var user1Model = new UserModel
            {
                Id = 11,
                Email = "User1@email.cz",
                FirstName = "Name1",
                LastName = "Surname1",
                Password = "Password1",
                UserDescription = "First user",
                ProfilePicture = new ProfileImageLightModel(),
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>()
            };

            var user2Model = new UserModel
            {
                Id = 12,
                Email = "User2@email.cz",
                FirstName = "Name2",
                LastName = "Surname2",
                Password = "Password2",
                UserDescription = "Second user",
                ProfilePicture = new ProfileImageLightModel(),
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>()
            };

            var returnedUser1 = RepositorySUT.Add(user1Model);
            var returnedUser2 = RepositorySUT.Add(user2Model);
            var allUsers = RepositorySUT.GetAll();

            Assert.NotNull(returnedUser1);
            Assert.NotNull(returnedUser2);
            Assert.NotEmpty(allUsers);

            RepositorySUT.Remove(returnedUser1.Id);
            RepositorySUT.Remove(returnedUser2.Id);
        }

        [Fact]
        public void GetById_WithExistingUser_DoesNotThrow()
        {
            var userModel = new UserModel
            {
                Id = 13,
                Email = "User1@email.cz",
                FirstName = "Name1",
                LastName = "Surname1",
                Password = "Password1",
                UserDescription = "First user",
                ProfilePicture = new ProfileImageLightModel(),
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>()
            };

            var returnedUser = RepositorySUT.Add(userModel);
            var foundUser = RepositorySUT.GetById(returnedUser.Id);

            Assert.NotNull(foundUser);
            Assert.Equal("Name1", foundUser.FirstName);

            RepositorySUT.Remove(returnedUser.Id);
        }

        [Fact]
        public void Remove_WithExistingUser_DoesNotThrow()
        {
            var userModel = new UserModel
            {
                Id = 15,
                Email = "User1@email.cz",
                FirstName = "Name1",
                LastName = "Surname1",
                Password = "Password1",
                UserDescription = "First user",
                ProfilePicture = new ProfileImageLightModel(),
                AdministratedTeams = new List<TeamLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>()
            };

            RepositorySUT.Add(userModel);
            RepositorySUT.Remove(userModel.Id);
            var returnedUser = RepositorySUT.GetById(userModel.Id);

            Assert.Null(returnedUser);
        }

        public void Dispose()
        {
            this.userRepositoryTestsFixture.TearDownDatabase();
        }
    }
}
