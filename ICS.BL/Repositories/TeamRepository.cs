using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities;
using TeamsManager.DAL.Entities.Contributions;

namespace TeamsManager.BL.Repositories
{
    public class TeamRepository : IRepository<TeamModel, TeamLightModel>
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public TeamRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<TeamLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .Teams
                .Select(mapper.MapTeamToTeamLightModel);
        }

        public TeamModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Teams
                .Include(t => t.Admin)
                .Include(t => t.TeamMembers)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.Author)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.Author)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.AssociatedFiles)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.ContributionUserTags)
                            .ThenInclude(cut => cut.User)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.AssociatedFiles)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.ContributionUserTags)
                        .ThenInclude(cut => cut.User)
                .Include(t => t.Posts)
                .FirstOrDefault(t => t.Id == id);

            return mapper.MapTeamToTeamModel(foundEntity);
        }

        public void Update(TeamModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapTeamModelToTeam(model);
                try
                {
                    dbContext.Update(entity);
                }
                catch
                {
                    var entry = dbContext.Entry(entity);
                    entry.State = EntityState.Modified;
                }
                finally
                {
                    dbContext.SaveChanges();
                }
            }
        }

        public TeamModel Add(TeamModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapTeamModelToTeam(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapTeamToTeamModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new Team
                {
                    Id = id
                };
                dbContext.Teams.Attach(entity);
                dbContext.Teams.Remove(entity);
                dbContext.SaveChanges();
            }
        }
        private static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State.ToString()}");
            }
        }
    }
}
