using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class UserRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public UserRepositoryTestsFixture() : base(nameof(UserRepositoryTestsFixture))
        {
            Repository = new UserRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public UserRepository Repository { get; }
    }
}
