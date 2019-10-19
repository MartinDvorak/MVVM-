using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities.Contributions;

namespace TeamsManager.BL.Repositories
{
    public class CommentRepository : IRepository<CommentModel, CommentLightModel>
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public CommentRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<CommentLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .Comments
                .Select(mapper.MapCommentToCommentLightModel);
        }

        public CommentModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Comments
                .Include(t => t.Author)
                .Include(t => t.AssociatedFiles)
                .Include(t => t.ContributionUserTags)
                .Include(t => t.ParentContribution)
                .FirstOrDefault(t => t.Id == id);

            return foundEntity == null ? null : mapper.MapCommentToCommentModel(foundEntity);
        }

        public void Update(CommentModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapCommentModelToComment(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public CommentModel Add(CommentModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapCommentModelToComment(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapCommentToCommentModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new Comment
                {
                    Id = id
                };
                dbContext.Comments.Attach(entity);
                dbContext.Comments.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
