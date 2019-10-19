using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities.Contributions;

namespace TeamsManager.BL.Repositories
{
    public class ContributionUserTagRepository : IRelationRepository<ContributionUserTagModel>
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public ContributionUserTagRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<ContributionUserTagModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .ContributionUserTags
                .Include(t => t.Contribution)
                .Include(t => t.User)
                .Select(mapper.MapContributionUserTagToContributionUserTagModel);
        }

        public ContributionUserTagModel GetById(int? userId, int? relatedId)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .ContributionUserTags
                .Include(t => t.Contribution)
                .Include(t => t.User)
                .FirstOrDefault(t => t.UserId == userId && t.ContributionId == relatedId);

            return foundEntity == null ? null : mapper.MapContributionUserTagToContributionUserTagModel(foundEntity);
        }

        public void Update(ContributionUserTagModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapContributionUserTagModelToContributionUserTag(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public ContributionUserTagModel Add(ContributionUserTagModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapContributionUserTagModelToContributionUserTag(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapContributionUserTagToContributionUserTagModel(entity);
            }
        }

        public void Remove(int? userId, int? relatedId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new ContributionUserTag
                {
                    UserId = userId,
                    ContributionId = relatedId
                };
                dbContext.ContributionUserTags.Attach(entity);
                dbContext.ContributionUserTags.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
