using System;
using System.Collections.Generic;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class TeamRepositoryTests : IClassFixture<TeamRepositoryTestsFixture>, IDisposable
    {
        private readonly TeamRepositoryTestsFixture teamRepositoryTestsFixture;

        private TeamRepository RepositorySUT => teamRepositoryTestsFixture.Repository;

        public TeamRepositoryTests(TeamRepositoryTestsFixture teamRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.teamRepositoryTestsFixture = teamRepositoryTestsFixture;

            this.teamRepositoryTestsFixture.PrepareDatabase();
        }
    
        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var teamModel = new TeamModel
            {
                Id = 10,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            var returnedTeam = RepositorySUT.Add(teamModel);

            Assert.NotNull(returnedTeam);
            RepositorySUT.Remove(returnedTeam.Id);

        }
        [Fact]
        public void GetAllTeams_WithExisting_DoesNotThrow()
        {
            var team1Model = new TeamModel
            {
                Id = 11,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            var team2Model = new TeamModel
            {
                Id = 12,
                Name = "Team2",
                Description = "Second team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            var returnedTeam1 = RepositorySUT.Add(team1Model);
            var returnedTeam2 = RepositorySUT.Add(team2Model);
            var allTeams = RepositorySUT.GetAll();

            Assert.NotNull(returnedTeam1);
            Assert.NotNull(returnedTeam2);
            Assert.NotEmpty(allTeams);

            RepositorySUT.Remove(returnedTeam1.Id);
            RepositorySUT.Remove(returnedTeam2.Id);
        }


        [Fact]
        public void GetById_WithExisting_DoesNotThrow()
        {
            var teamModel = new TeamModel
            {
                Id = 13,
                Name = "Team1",
                Description = "First team",
                Admin = new UserLightModel(),
                Members = new List<UserTeamMemberModel>(),
                Posts = new List<PostLightModel>()
            };

            var returnedTeam = RepositorySUT.Add(teamModel);
            var foundTeam = RepositorySUT.GetById(returnedTeam.Id);

            Assert.NotNull(foundTeam);
            Assert.Equal("Team1", foundTeam.Name);

            RepositorySUT.Remove(foundTeam.Id);
        }

        public void Dispose()
        {
            this.teamRepositoryTestsFixture.TearDownDatabase();
        }
    }
}
