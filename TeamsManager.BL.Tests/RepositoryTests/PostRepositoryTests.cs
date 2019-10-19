using System;
using System.Collections.Generic;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{

    public class PostRepositoryTests : IClassFixture<PostRepositoryTestsFixture>, IDisposable
    {
        private readonly PostRepositoryTestsFixture postRepositoryTestsFixture;

        private PostRepository RepositorySUT => postRepositoryTestsFixture.Repository;

        public PostRepositoryTests(PostRepositoryTestsFixture postRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.postRepositoryTestsFixture = postRepositoryTestsFixture;

            this.postRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 10,
                Title = "Title1",
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var returnedPost = RepositorySUT.Add(postModel);

            Assert.NotNull(returnedPost);
            RepositorySUT.Remove(returnedPost.Id);
        }

        [Fact]
        public void GetAll_WithExistingPost_DoesNotThrow()
        {
            var post1Model = new PostModel
            {
                Id = 11,
                Title = "Title1",
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var post2Model = new PostModel
            {
                Id = 12,
                Title = "Title2",
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var returnedPost1 = RepositorySUT.Add(post1Model);
            var returnedPost2 = RepositorySUT.Add(post2Model);
            var allPosts = RepositorySUT.GetAll();

            Assert.NotNull(returnedPost1);
            Assert.NotNull(returnedPost2);
            Assert.NotEmpty(allPosts);

            RepositorySUT.Remove(returnedPost1.Id);
            RepositorySUT.Remove(returnedPost2.Id);
        }

        [Fact]
        public void GetById_WithExistingPost_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 13,
                Title = "Title1",
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            var returnedPost = RepositorySUT.Add(postModel);
            var foundPost = RepositorySUT.GetById(returnedPost.Id);

            Assert.NotNull(foundPost);
            Assert.Equal("Title1", foundPost.Title);

            RepositorySUT.Remove(foundPost.Id);
        }

        [Fact]
        public void Remove_WithExistingPost_DoesNotThrow()
        {
            var postModel = new PostModel
            {
                Id = 15,
                Title = "Title1",
                CorrespondingTeam = new TeamLightModel(),
                Comments = new List<CommentModel>()
            };

            RepositorySUT.Add(postModel);
            RepositorySUT.Remove(postModel.Id);
            var returnedPost = RepositorySUT.GetById(postModel.Id);

            Assert.Null(returnedPost);
        }


        public void Dispose()
        {
            this.postRepositoryTestsFixture.TearDownDatabase();
        }
    }
}