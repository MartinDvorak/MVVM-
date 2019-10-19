using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Repositories
{
    public class ContributionFileRepository : IRepository<ContributionFileModel, ContributionFileLightModel>
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public ContributionFileRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<ContributionFileLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .ContributionFiles
                .Select(mapper.MapContributionFileToContributionFileLightModel);
        }

        public ContributionFileModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .ContributionFiles
                .Include(t => t.AssociatedContribution)
                .FirstOrDefault(t => t.Id == id);

            return foundEntity == null ? null : mapper.MapContributionFileToContributionFileModel(foundEntity);
        }

        public void Update(ContributionFileModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapContributionFileModelToContributionFile(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public ContributionFileModel Add(ContributionFileModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapContributionFileModelToContributionFile(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapContributionFileToContributionFileModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new ContributionFile()
                {
                    Id = id
                };
                dbContext.ContributionFiles.Attach(entity);
                dbContext.ContributionFiles.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
