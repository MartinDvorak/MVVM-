using System;
using System.Linq;
using TeamsManager.DAL.Entities.Files;
using Xunit;

namespace TeamsManager.DAL.Tests
{
    public class TeamsManagerDbContextTests : IClassFixture<TeamsManagerDbContextTestsClassSetupFixture>, IDisposable
    {
        public TeamsManagerDbContextTests(TeamsManagerDbContextTestsClassSetupFixture testContext)
        {
            _testContext = testContext;

            testContext.PrepareDatabase();
        }

        // ReSharper disable once InconsistentNaming
        private readonly TeamsManagerDbContextTestsClassSetupFixture _testContext;


        [Fact]
        public void AddProfileImageTest()
        {
            //Arrange
            var profileImageEntity = new ProfileImage
            {
                Id = 10,
                Content = new byte[10],
                FileName = "Profile picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            //Act
            _testContext.TeamsManagerDbContextSUT.ProfileImages.Add(profileImageEntity);
            _testContext.TeamsManagerDbContextSUT.SaveChanges();


            //Assert
            using (var dbx = _testContext.DbContextFactory.CreateDbContext())
            {
                var retrievedProfileImage = dbx.ProfileImages.First(entity => entity.Id == profileImageEntity.Id);
                Assert.Equal(profileImageEntity.FileName, retrievedProfileImage.FileName);
            }
        }

        [Fact]
        public void AddProfileImageTest2()
        {
            //Arrange
            var profileImageEntity = new ProfileImage
            {
                Id = 11,
                Content = new byte[10],
                FileName = "Profile picture",
                PictureFormat = SupportedFormatPicture.Png
            };

            //Act
            _testContext.TeamsManagerDbContextSUT.ProfileImages.Add(profileImageEntity);
            _testContext.TeamsManagerDbContextSUT.SaveChanges();


            //Assert
            using (var dbx = _testContext.DbContextFactory.CreateDbContext())
            {
                var retrievedProfileImage = dbx.ProfileImages.First(entity => entity.Id == profileImageEntity.Id);
                Assert.Equal(profileImageEntity.FileName, retrievedProfileImage.FileName);
            }
        }

        public void Dispose()
        {
            _testContext.TearDownDatabase();
        }
    }
}
