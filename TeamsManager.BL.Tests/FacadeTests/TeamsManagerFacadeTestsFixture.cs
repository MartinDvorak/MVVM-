using TeamsManager.BL.Facade;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.FacadeTests
{
    public class TeamsManagerFacadeTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public TeamsManagerFacadeTestsFixture() : base(nameof(TeamsManagerFacadeTestsFixture))
        {
            Facade = new BusinessLogicFacade(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }
        public IBusinessLogicFacade Facade { get; }
    }
}