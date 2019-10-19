using System;
using Microsoft.EntityFrameworkCore;

namespace TeamsManager.DAL.DbContext
{
    public class InMemoryDbContextFactory : IDbContextFactory
    {
        private readonly String testDbName;

        public InMemoryDbContextFactory(String testDbName) => this.testDbName = testDbName;

        public TeamsManagerDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TeamsManagerDbContext>();
            optionsBuilder.UseInMemoryDatabase(this.testDbName);

            //optionsBuilder.UseSqlServer($@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = {this.testDbName};MultipleActiveResultSets = True;Integrated Security = True; ");

            optionsBuilder.EnableSensitiveDataLogging();
            return new TeamsManagerSeedingDbContext(optionsBuilder.Options);
        }
    }
}