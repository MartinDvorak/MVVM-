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
    public class UserTeamMemberRepositoryTests : IClassFixture<UserTeamMemberRepositoryTestsFixture>, IDisposable
    {
        private readonly UserTeamMemberRepositoryTestsFixture userTeamMemberRepositoryTestsFixture;
        private UserTeamMemberRepository RepositorySUT => userTeamMemberRepositoryTestsFixture.Repository;
        private UserRepository UserRepositorySUT => userTeamMemberRepositoryTestsFixture.UserRepository;
        private TeamRepository TeamRepositorySUT => userTeamMemberRepositoryTestsFixture.TeamRepository;

        public UserTeamMemberRepositoryTests(UserTeamMemberRepositoryTestsFixture userTeamMemberRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.userTeamMemberRepositoryTestsFixture = userTeamMemberRepositoryTestsFixture;

            this.userTeamMemberRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var user1Model = new UserModel
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
            user1Model = UserRepositorySUT.Add(user1Model);

            var team1Model = new TeamModel
            {
                Id = 11,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };
            team1Model = TeamRepositorySUT.Add(team1Model);


            var userTeamMemberModel = new UserTeamMemberModel
            {
                UserId = user1Model.Id,
                TeamId = team1Model.Id,
                User = new UserLightModel(),
                Team = new TeamLightModel()
            };

            var returnedUserTeamMember = RepositorySUT.Add(userTeamMemberModel);

            Assert.NotNull(returnedUserTeamMember);
            RepositorySUT.Remove(returnedUserTeamMember.UserId, returnedUserTeamMember.TeamId);
        }

        [Fact]
        public void GetAll_WithExistingUserTeamMember_DoesNotThrow()
        {
            var user1Model = new UserModel
            {
                Id = 12,
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
            UserRepositorySUT.Add(user1Model);

            var user2Model = new UserModel
            {
                Id = 13,
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
            UserRepositorySUT.Add(user2Model);

            var team1Model = new TeamModel
            {
                Id = 14,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };
            TeamRepositorySUT.Add(team1Model);

            var team2Model = new TeamModel
            {
                Id = 15,
                Name = "Team2",
                Description = "Second team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };
            TeamRepositorySUT.Add(team2Model);

            var userTeamMember1Model = new UserTeamMemberModel
            {
                UserId = user1Model.Id,
                TeamId = user2Model.Id,
                User = new UserLightModel(),
                Team = new TeamLightModel()
            };

            var userTeamMember2Model = new UserTeamMemberModel
            {
                UserId = user2Model.Id,
                TeamId = team2Model.Id,
                User = new UserLightModel(),
                Team = new TeamLightModel()
            };

            var returnedUserTeamMember1 = RepositorySUT.Add(userTeamMember1Model);
            var returnedUserTeamMember2 = RepositorySUT.Add(userTeamMember2Model);
            var allUserTeamMembers = RepositorySUT.GetAll();

            Assert.NotNull(returnedUserTeamMember1);
            Assert.NotNull(returnedUserTeamMember2);
            Assert.NotEmpty(allUserTeamMembers);

            RepositorySUT.Remove(returnedUserTeamMember1.UserId, returnedUserTeamMember1.TeamId);
            RepositorySUT.Remove(returnedUserTeamMember2.UserId, returnedUserTeamMember2.TeamId);
            UserRepositorySUT.Remove(user1Model.Id);
            UserRepositorySUT.Remove(user2Model.Id);
            TeamRepositorySUT.Remove(team1Model.Id);
            TeamRepositorySUT.Remove(team2Model.Id);
        }

        [Fact]
        public void GetById_WithExistingUserTeamMember_DoesNotThrow()
        {
            var user1Model = new UserModel
            {
                Id = 16,
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
            user1Model = UserRepositorySUT.Add(user1Model);

            var team1Model = new TeamModel
            {
                Id = 17,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };
            team1Model = TeamRepositorySUT.Add(team1Model);

            var userTeamMemberModel = new UserTeamMemberModel
            {
                UserId = user1Model.Id,
                TeamId = team1Model.Id,
                User = new UserLightModel(),
                Team = new TeamLightModel()
            };

            var returnedUserTeamMember = RepositorySUT.Add(userTeamMemberModel);
            var foundUserTeamMember = RepositorySUT.GetById(returnedUserTeamMember.UserId, returnedUserTeamMember.TeamId);

            Assert.NotNull(foundUserTeamMember);
            Assert.Equal(user1Model.Id, foundUserTeamMember.UserId);

            RepositorySUT.Remove(returnedUserTeamMember.UserId, returnedUserTeamMember.TeamId);
            UserRepositorySUT.Remove(user1Model.Id);
            TeamRepositorySUT.Remove(team1Model.Id);
        }

        [Fact]
        public void Remove_WithExistingUserTeamMember_DoesNotThrow()
        {
            var user1Model = new UserModel
            {
                Id = 18,
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
            UserRepositorySUT.Add(user1Model);

            var team1Model = new TeamModel
            {
                Id = 19,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };
            TeamRepositorySUT.Add(team1Model);

            var userTeamMemberModel = new UserTeamMemberModel
            {
                UserId = user1Model.Id,
                TeamId = team1Model.Id,
                User = new UserLightModel(),
                Team = new TeamLightModel()
            };

            RepositorySUT.Add(userTeamMemberModel);
            RepositorySUT.Remove(userTeamMemberModel.UserId, userTeamMemberModel.TeamId);
            var returnedUserTeamMember = RepositorySUT.GetById(userTeamMemberModel.UserId, userTeamMemberModel.TeamId);

            Assert.Null(returnedUserTeamMember);
            UserRepositorySUT.Remove(user1Model.Id);
            TeamRepositorySUT.Remove(team1Model.Id);
        }

        public void Dispose()
        {
            this.userTeamMemberRepositoryTestsFixture.TearDownDatabase();
        }
    }
}