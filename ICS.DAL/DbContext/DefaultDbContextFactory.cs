using Microsoft.EntityFrameworkCore;

namespace TeamsManager.DAL.DbContext
{
    public class DefaultDbContextFactory : IDbContextFactory
    {
        public TeamsManagerDbContext CreateDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder<TeamsManagerDbContext>();
            optionBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = TeamsManager;MultipleActiveResultSets = True;Integrated Security = True");
            return new TeamsManagerDbContext(optionBuilder.Options);
        }
    }
}
