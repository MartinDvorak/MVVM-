using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class PostRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public PostRepositoryTestsFixture() : base(nameof(PostRepositoryTestsFixture))
        {
            Repository = new PostRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public PostRepository Repository { get; }
    }
}
