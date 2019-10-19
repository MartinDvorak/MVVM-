using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class CommentRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public CommentRepositoryTestsFixture() : base(nameof(CommentRepositoryTestsFixture))
        {
            Repository = new CommentRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public CommentRepository Repository { get; }
    }
}
