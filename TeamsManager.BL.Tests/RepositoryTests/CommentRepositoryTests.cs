using System;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{

    public class CommentRepositoryTests : IClassFixture<CommentRepositoryTestsFixture>, IDisposable
    {
        private readonly CommentRepositoryTestsFixture commentRepositoryTestsFixture;

        private CommentRepository RepositorySUT => commentRepositoryTestsFixture.Repository;

        public CommentRepositoryTests(CommentRepositoryTestsFixture commentRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.commentRepositoryTestsFixture = commentRepositoryTestsFixture;

            this.commentRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var commentModel = new CommentModel
            {
                Id = 10,
                ParentContribution = new PostLightModel()
            };

            var returnedComment = RepositorySUT.Add(commentModel);

            Assert.NotNull(returnedComment);
            RepositorySUT.Remove(returnedComment.Id);
        }

        [Fact]
        public void GetAll_WithExistingComment_DoesNotThrow()
        {
            var comment1Model = new CommentModel
            {
                Id = 11,
                ParentContribution = new PostLightModel()
            };

            var comment2Model = new CommentModel
            {
                Id = 12,
                ParentContribution = new PostLightModel()
            };

            var returnedComment1 = RepositorySUT.Add(comment1Model);
            var returnedComment2 = RepositorySUT.Add(comment2Model);
            var allComments = RepositorySUT.GetAll();

            Assert.NotNull(returnedComment1);
            Assert.NotNull(returnedComment2);
            Assert.NotEmpty(allComments);

            RepositorySUT.Remove(returnedComment1.Id);
            RepositorySUT.Remove(returnedComment2.Id);
        }

        [Fact]
        public void GetById_WithExistingComment_DoesNotThrow()
        {
            var commentModel = new CommentModel
            {
                Id = 13,
                ParentContribution = new PostLightModel()
            };

            var returnedComment = RepositorySUT.Add(commentModel);
            var foundComment = RepositorySUT.GetById(returnedComment.Id);

            Assert.NotNull(foundComment);
            Assert.Equal(commentModel.Id, foundComment.Id);

            RepositorySUT.Remove(returnedComment.Id);
        }

        [Fact]
        public void Remove_WithExistingComment_DoesNotThrow()
        {
            var commentModel = new CommentModel
            {
                Id = 14,
                ParentContribution = new PostLightModel()
            };

            RepositorySUT.Add(commentModel);
            RepositorySUT.Remove(commentModel.Id);
            var returnedComment = RepositorySUT.GetById(commentModel.Id);

            Assert.Null(returnedComment);
        }

        public void Dispose()
        {
            this.commentRepositoryTestsFixture.TearDownDatabase();
        }
    }
}