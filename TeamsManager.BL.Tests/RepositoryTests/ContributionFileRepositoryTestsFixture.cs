using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.Tests;

namespace TeamsManager.BL.Tests.RepositoryTests
{
    public class ContributionFileRepositoryTestsFixture : TeamsManagerDbContextSetupFixture
    {
        public ContributionFileRepositoryTestsFixture() : base(nameof(ContributionFileRepositoryTestsFixture))
        {
            Repository = new ContributionFileRepository(DbContextFactory, new Mapper.Mapper());

            PrepareDatabase();
        }

        public ContributionFileRepository Repository { get; }
    }
}
