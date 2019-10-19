namespace TeamsManager.DAL.DbContext
{
    public interface IDbContextFactory
    {
        TeamsManagerDbContext CreateDbContext();
    }

}
