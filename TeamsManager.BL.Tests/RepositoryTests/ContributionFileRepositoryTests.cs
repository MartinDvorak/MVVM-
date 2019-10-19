using System;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Entities.Files;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{

    public class ContributionFileRepositoryTests : IClassFixture<ContributionFileRepositoryTestsFixture>, IDisposable
    {
        private readonly ContributionFileRepositoryTestsFixture contributionFileRepositoryTestsFixture;

        private ContributionFileRepository RepositorySUT => contributionFileRepositoryTestsFixture.Repository;

        public ContributionFileRepositoryTests(ContributionFileRepositoryTestsFixture contributionFileRepositoryTestsFixture, ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.contributionFileRepositoryTestsFixture = contributionFileRepositoryTestsFixture;

            this.contributionFileRepositoryTestsFixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var contributionFileModel = new ContributionFileModel
            {
                Id = 10,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.Zip
            };

            var returnedContributionFile = RepositorySUT.Add(contributionFileModel);

            Assert.NotNull(returnedContributionFile);
            RepositorySUT.Remove(returnedContributionFile.Id);
        }

        [Fact]
        public void GetAll_WithExistingContributionFile_DoesNotThrow()
        {
            var contributionFile1Model = new ContributionFileModel
            {
                Id = 11,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.Zip
            };

            var contributionFile2Model = new ContributionFileModel
            {
                Id = 12,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.TarGz
            };

            var returnedContributionFile1 = RepositorySUT.Add(contributionFile1Model);
            var returnedContributionFile2 = RepositorySUT.Add(contributionFile2Model);
            var allContributionFiles = RepositorySUT.GetAll();

            Assert.NotNull(returnedContributionFile1);
            Assert.NotNull(returnedContributionFile2);
            Assert.NotEmpty(allContributionFiles);

            RepositorySUT.Remove(returnedContributionFile1.Id);
            RepositorySUT.Remove(returnedContributionFile2.Id);
        }

        [Fact]
        public void GetById_WithExistingContributionFile_DoesNotThrow()
        {
            var contributionFileModel = new ContributionFileModel
            {
                Id = 13,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.Zip
            };

            var returnedContributionFile = RepositorySUT.Add(contributionFileModel);
            var foundContributionFile = RepositorySUT.GetById(returnedContributionFile.Id);

            Assert.NotNull(foundContributionFile);
            Assert.Equal(returnedContributionFile.Id, foundContributionFile.Id);

            RepositorySUT.Remove(foundContributionFile.Id);
        }

        [Fact]
        public void ChangeFormat_WithExistingContributionFile_DoesNotThrow()
        {
            var contributionFileModel = new ContributionFileModel
            {
                Id = 14,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.Zip
            };

            var addedContributionFile = RepositorySUT.Add(contributionFileModel);
            addedContributionFile.FileFormat = SupportedFormatFile.Rar;
            RepositorySUT.Update(addedContributionFile);

            var updatedContributionFile = RepositorySUT.GetById(addedContributionFile.Id);
            Assert.NotNull(updatedContributionFile);
            Assert.Equal(SupportedFormatFile.Rar, updatedContributionFile.FileFormat);

            RepositorySUT.Remove(updatedContributionFile.Id);
        }

        [Fact]
        public void Remove_WithExistingContributionFile_DoesNotThrow()
        {
            var contributionFileModel = new ContributionFileModel
            {
                Id = 15,
                AssociatedContribution = new CommentLightModel(),
                FileFormat = SupportedFormatFile.Zip
            };

            RepositorySUT.Add(contributionFileModel);
            RepositorySUT.Remove(contributionFileModel.Id);
            var returnedContributionFile = RepositorySUT.GetById(contributionFileModel.Id);

            Assert.Null(returnedContributionFile);
        }

        public void Dispose()
        {
            contributionFileRepositoryTestsFixture.TearDownDatabase();
        }
    }
}