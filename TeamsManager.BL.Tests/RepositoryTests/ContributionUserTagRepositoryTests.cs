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

    public class ContributionUserTagRepositoryTests : IClassFixture<ContributionUserTagRepositoryTestsFixture>, IDisposable
    {
        private readonly ContributionUserTagRepositoryTestsFixture contributionUserTagRepositoryTestsFixture;

        private ContributionUserTagRepository RepositorySUT => contributionUserTagRepositoryTestsFixture.Repository;
        private UserRepository UserRepositorySUT => contributionUserTagRepositoryTestsFixture.UserRepository;
        private PostRepository PostRepositorySUT => contributionUserTagRepositoryTestsFixture.PostRepository;

        public ContributionUserTagRepositoryTests(ContributionUserTagRepositoryTestsFixture contributionUserTagRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.contributionUserTagRepositoryTestsFixture = contributionUserTagRepositoryTestsFixture;

            this.contributionUserTagRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 10,
                Title = "title",
                Content = "content",
                Date = new DateTime(),
                Author = new UserLightModel(),
                AssociatedFiles = new List<ContributionFileLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var userModel = new UserModel
            {
                Id = 11,
                Email = "email",
                FirstName = "name",
                LastName = "surname",
                Password = "***",
                UserDescription = "desc",
                ProfilePicture = new ProfileImageLightModel(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                AdministratedTeams = new List<TeamLightModel>()
            };

            var returnedPost = PostRepositorySUT.Add(postModel);
            var returnedUser = UserRepositorySUT.Add(userModel);

            var contributionUserTagModel = new ContributionUserTagModel
            {
                ContributionId = returnedPost.Id,
                UserId = returnedUser.Id,
                Contribution = new PostLightModel(),
                User = new UserLightModel()
            };

            var returnedContributionUserTag = RepositorySUT.Add(contributionUserTagModel);

            Assert.NotNull(returnedContributionUserTag);
            RepositorySUT.Remove(returnedContributionUserTag.UserId, returnedContributionUserTag.ContributionId);
            PostRepositorySUT.Remove(returnedPost.Id);
            UserRepositorySUT.Remove(returnedUser.Id);
        }

        [Fact]
        public void GetAll_WithExistingContributionUserTag_DoesNotThrow()
        {
            var postModel1 = new PostModel
            {
                Id = 12,
                Title = "title",
                Content = "content",
                Date = new DateTime(),
                Author = new UserLightModel(),
                AssociatedFiles = new List<ContributionFileLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var userModel1 = new UserModel
            {
                Id = 13,
                Email = "email",
                FirstName = "name",
                LastName = "surname",
                Password = "***",
                UserDescription = "desc",
                ProfilePicture = new ProfileImageLightModel(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                AdministratedTeams = new List<TeamLightModel>()
            };

            var returnedPost1 = PostRepositorySUT.Add(postModel1);
            var returnedUser1 = UserRepositorySUT.Add(userModel1);

            var postModel2 = new PostModel
            {
                Id = 14,
                Title = "title",
                Content = "content",
                Date = new DateTime(),
                Author = new UserLightModel(),
                AssociatedFiles = new List<ContributionFileLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var userModel2 = new UserModel
            {
                Id = 15,
                Email = "email",
                FirstName = "name",
                LastName = "surname",
                Password = "***",
                UserDescription = "desc",
                ProfilePicture = new ProfileImageLightModel(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                AdministratedTeams = new List<TeamLightModel>()
            };

            var returnedPost2 = PostRepositorySUT.Add(postModel2);
            var returnedUser2 = UserRepositorySUT.Add(userModel2);

            var contributionUserTag1Model = new ContributionUserTagModel
            {
                ContributionId = returnedPost1.Id,
                UserId = returnedUser1.Id,
                Contribution = new PostLightModel(),
                User = new UserLightModel()
            };

            var contributionUserTag2Model = new ContributionUserTagModel
            {
                ContributionId = returnedPost2.Id,
                UserId = returnedUser2.Id,
                Contribution = new PostLightModel(),
                User = new UserLightModel()
            };

            var returnedContributionUserTag1 = RepositorySUT.Add(contributionUserTag1Model);
            var returnedContributionUserTag2 = RepositorySUT.Add(contributionUserTag2Model);
            var allContributionUserTags = RepositorySUT.GetAll();

            Assert.NotNull(returnedContributionUserTag1);
            Assert.NotNull(returnedContributionUserTag2);
            Assert.NotEmpty(allContributionUserTags);

            RepositorySUT.Remove(returnedContributionUserTag1.UserId, returnedContributionUserTag1.ContributionId);
            RepositorySUT.Remove(returnedContributionUserTag2.UserId, returnedContributionUserTag2.ContributionId);
            PostRepositorySUT.Remove(returnedPost1.Id);
            PostRepositorySUT.Remove(returnedPost2.Id);
            UserRepositorySUT.Remove(returnedUser1.Id);
            UserRepositorySUT.Remove(returnedUser2.Id);
        }

        [Fact]
        public void GetById_WithExistingContributionUserTag_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 16,
                Title = "title",
                Content = "content",
                Date = new DateTime(),
                Author = new UserLightModel(),
                AssociatedFiles = new List<ContributionFileLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var userModel = new UserModel
            {
                Id = 17,
                Email = "email",
                FirstName = "name",
                LastName = "surname",
                Password = "***",
                UserDescription = "desc",
                ProfilePicture = new ProfileImageLightModel(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                AdministratedTeams = new List<TeamLightModel>()
            };

            var returnedPost = PostRepositorySUT.Add(postModel);
            var returnedUser = UserRepositorySUT.Add(userModel);

            var contributionUserTagModel = new ContributionUserTagModel
            {
                ContributionId = returnedPost.Id,
                UserId = returnedUser.Id,
                Contribution = new PostLightModel(),
                User = new UserLightModel()
            };

            var returnedContributionUserTag = RepositorySUT.Add(contributionUserTagModel);
            var foundContributionUserTag = RepositorySUT.GetById(returnedContributionUserTag.UserId, returnedContributionUserTag.ContributionId);

            Assert.NotNull(foundContributionUserTag);
            Assert.Equal(postModel.Id, foundContributionUserTag.ContributionId);

            RepositorySUT.Remove(returnedContributionUserTag.UserId, returnedContributionUserTag.ContributionId);
            PostRepositorySUT.Remove(returnedPost.Id);
            UserRepositorySUT.Remove(returnedUser.Id);
        }

        [Fact]
        public void Remove_WithExistingContributionUserTag_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 18,
                Title = "title",
                Content = "content",
                Date = new DateTime(),
                Author = new UserLightModel(),
                AssociatedFiles = new List<ContributionFileLightModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };
            PostRepositorySUT.Add(postModel);

            var userModel = new UserModel
            {
                Id = 19,
                Email = "email",
                FirstName = "name",
                LastName = "surname",
                Password = "***",
                UserDescription = "desc",
                ProfilePicture = new ProfileImageLightModel(),
                MyContributions = new List<ContributionLightModel>(),
                UserTeams = new List<UserTeamMemberModel>(),
                ContributionUserTags = new List<ContributionUserTagModel>(),
                AdministratedTeams = new List<TeamLightModel>()
            };
            UserRepositorySUT.Add(userModel);

            var contributionUserTagModel = new ContributionUserTagModel
            {
                ContributionId = postModel.Id,
                UserId = userModel.Id,
                Contribution = new PostLightModel(),
                User = new UserLightModel()
            };

            RepositorySUT.Add(contributionUserTagModel);
            RepositorySUT.Remove(contributionUserTagModel.UserId, contributionUserTagModel.ContributionId);
            var returnedContributionUserTag = RepositorySUT.GetById(contributionUserTagModel.UserId, contributionUserTagModel.ContributionId);

            Assert.Null(returnedContributionUserTag);

            UserRepositorySUT.Remove(userModel.Id);
            PostRepositorySUT.Remove(postModel.Id);

        }

        public void Dispose()
        {
            contributionUserTagRepositoryTestsFixture.TearDownDatabase();
        }
    }
}