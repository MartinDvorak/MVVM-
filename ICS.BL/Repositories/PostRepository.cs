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
    public class PostRepository : IRepository<PostModel, PostLightModel>
    {

        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public PostRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }

        public IEnumerable<PostLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .Posts
                .Include(t => t.Comments)
                .Select(mapper.MapPostToPostLightModel);
        }

        public PostModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .Posts
                .Include(t => t.Author)
                .Include(t => t.AssociatedFiles)
                .Include(t => t.ContributionUserTags)
                .Include(t => t.CorrespondingTeam)
                .Include(t => t.ContributionUserTags)
                    .ThenInclude(r => r.User)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.Author)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.ContributionUserTags)
                        .ThenInclude(cut => cut.User)
                .FirstOrDefault(t => t.Id == id);

            return foundEntity == null ? null : mapper.MapPostToPostModel(foundEntity);
        }

        public void Update(PostModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapPostModelToPost(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public PostModel Add(PostModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapPostModelToPost(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapPostToPostModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = new Post
                {
                    Id = id
                };
                dbContext.Posts.Attach(entity);
                dbContext.Posts.Remove(entity);
                dbContext.SaveChanges();
            }
        }
    }
}
