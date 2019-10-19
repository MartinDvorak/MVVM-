using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities;

namespace TeamsManager.BL.Repositories
{
    public class UserRepository : IRepository<UserModel, UserLightModel>
    {

        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public UserRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<UserLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .Users
                .Include(t => t.Photo)
                .Select(mapper.MapUserToUserLightModel);
        }

        public UserModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Users
                .Include(t => t.Photo)
                .Include(t => t.MyContributions)
                .Include(t => t.UserTeams)
                .Include(t => t.AdministratedTeams)
                .Include(t => t.ContributionUserTags)
                .FirstOrDefault(t => t.Id == id);

            if (foundEntity == null)
                return null;

            return mapper.MapUserToUserModel(foundEntity);
        }

        public void Update(UserModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                try
                {
                    var entity = mapper.MapUserModelToUser(model);
                    dbContext.Update(entity);
                    dbContext.SaveChanges();
                }
                catch {}
            }
        }

        public UserModel Add(UserModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapUserModelToUser(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapUserToUserModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new User
                {
                    Id = id
                };
                dbContext.Users.Attach(entity);
                dbContext.Users.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}