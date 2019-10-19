using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class ProfileImageRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public ProfileImageRepositoryTestsFixture() : base(nameof(ProfileImageRepositoryTestsFixture))
        {
            Repository = new ProfileImageRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public ProfileImageRepository Repository { get; }
    }
}
