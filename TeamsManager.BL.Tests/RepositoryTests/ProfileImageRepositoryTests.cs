using System;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Entities.Files;
using Xunit;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class ProfileImageRepositoryTests : IClassFixture<ProfileImageRepositoryTestsFixture>, IDisposable
    {
        private readonly ProfileImageRepositoryTestsFixture profileImageRepositoryTestsfixture;

        private ProfileImageRepository RepositorySUT => profileImageRepositoryTestsfixture.Repository;

        public ProfileImageRepositoryTests(ProfileImageRepositoryTestsFixture profileImageRepositoryTestsFixture,
            ITestOutputHelper output)
        {
            var converter = new XUnitTestOutputConverter(output);
            Console.SetOut(converter);
            this.profileImageRepositoryTestsfixture = profileImageRepositoryTestsFixture;

            this.profileImageRepositoryTestsfixture.PrepareDatabase();
        }

        [Fact]
        public void Create_NonExisting_DoesNotThrow()
        {
            var profileImageModel = new ProfileImageModel
            {
                Id = 10,
                PictureFormat = SupportedFormatPicture.Png
            };

            var returnedProfileImage = RepositorySUT.Add(profileImageModel);

            Assert.NotNull(returnedProfileImage);
            RepositorySUT.Remove(returnedProfileImage.Id);
        }

        [Fact]
        public void GetAll_WithExistingProfileImage_DoesNotThrow()
        {
            var profileImage1Model = new ProfileImageModel
            {
                Id = 11,
                PictureFormat = SupportedFormatPicture.Png
            };

            var profileImage2Model = new ProfileImageModel
            {
                Id = 12,
                PictureFormat = SupportedFormatPicture.Jpg
            };

            var returnedProfileImage1 = RepositorySUT.Add(profileImage1Model);
            var returnedProfileImage2 = RepositorySUT.Add(profileImage2Model);
            var allProfileImages = RepositorySUT.GetAll();

            Assert.NotNull(returnedProfileImage1);
            Assert.NotNull(returnedProfileImage2);
            Assert.NotEmpty(allProfileImages);

            RepositorySUT.Remove(returnedProfileImage1.Id);
            RepositorySUT.Remove(returnedProfileImage2.Id);
        }

        [Fact]
        public void GetById_WithExistingProfileImage_DoesNotThrow()
        {
            var profileImageModel = new ProfileImageModel
            {
                Id = 13,
                PictureFormat = SupportedFormatPicture.Png
            };

            var returnedProfileImage = RepositorySUT.Add(profileImageModel);
            var foundProfileImage = RepositorySUT.GetById(returnedProfileImage.Id);

            Assert.NotNull(foundProfileImage);
            Assert.Equal(profileImageModel.Id, foundProfileImage.Id);

            RepositorySUT.Remove(returnedProfileImage.Id);
        }

        [Fact]
        public void ChangeFormat_WithExistingProfileImage_DoesNotThrow()
        {
            var profileImageModel = new ProfileImageModel
            {
                Id = 14,
                PictureFormat = SupportedFormatPicture.Png
            };

            var addedProfileImage = RepositorySUT.Add(profileImageModel);
            addedProfileImage.PictureFormat = SupportedFormatPicture.Gif;
            RepositorySUT.Update(addedProfileImage);

            var updatedProfileImage = RepositorySUT.GetById(addedProfileImage.Id);
            Assert.NotNull(updatedProfileImage);
            Assert.Equal(SupportedFormatPicture.Gif, updatedProfileImage.PictureFormat);

            RepositorySUT.Remove(updatedProfileImage.Id);
        }

        [Fact]
        public void Remove_WithExistingProfileImage_DoesNotThrow()
        {
            var profileImageModel = new ProfileImageModel
            {
                Id = 15,
                PictureFormat = SupportedFormatPicture.Png
            };

            RepositorySUT.Add(profileImageModel);
            RepositorySUT.Remove(profileImageModel.Id);
            var returnedProfileImage = RepositorySUT.GetById(profileImageModel.Id);

            Assert.Null(returnedProfileImage);
        }

        public void Dispose()
        {
            profileImageRepositoryTestsfixture.TearDownDatabase();
        }
    }
}
