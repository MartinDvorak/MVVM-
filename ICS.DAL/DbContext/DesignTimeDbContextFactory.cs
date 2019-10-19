using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TeamsManager.DAL.DbContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TeamsManagerDbContext>
    {
        public TeamsManagerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TeamsManagerDbContext>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog = TeamsManager;MultipleActiveResultSets = True;Integrated Security = True");
            return new TeamsManagerDbContext(optionsBuilder.Options);
        }
    }
}
