using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities;

namespace TeamsManager.BL.Repositories
{
    public class UserTeamMemberRepository : IRelationRepository<UserTeamMemberModel>
    {

        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public UserTeamMemberRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }
        public IEnumerable<UserTeamMemberModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .UserTeamMembers
                .Include(t => t.User)
                .Include(t => t.Team)
                .Select(mapper.MapUserTeamMemberToUserTeamMemberModel);
        }

        public UserTeamMemberModel GetById(int? userId, int? relatedId)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .UserTeamMembers
                .Include(t => t.User)
                .Include(t => t.Team)
                .FirstOrDefault(t => t.UserId == userId && t.TeamId == relatedId);

            return foundEntity == null ? null : mapper.MapUserTeamMemberToUserTeamMemberModel(foundEntity);
        }

        public void Update(UserTeamMemberModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapUserTeamMemberModelToUserTeamMember(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public UserTeamMemberModel Add(UserTeamMemberModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapUserTeamMemberModelToUserTeamMember(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapUserTeamMemberToUserTeamMemberModel(entity);
            }
        }

        public void Remove(int? userId, int? relatedId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new UserTeamMember
                {
                    UserId = userId,
                    TeamId = relatedId
                };
                dbContext.UserTeamMembers.Attach(entity);
                dbContext.UserTeamMembers.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
