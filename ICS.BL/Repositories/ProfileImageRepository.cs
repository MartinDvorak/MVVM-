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
    public class ProfileImageRepository : IRepository<ProfileImageModel, ProfileImageLightModel>
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IMapper mapper;

        public ProfileImageRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.dbContextFactory = dbContextFactory;
            this.mapper = mapper;
        }
        public IEnumerable<ProfileImageLightModel> GetAll()
        {
            return dbContextFactory
                .CreateDbContext()
                .ProfileImages
                .Select(mapper.MapProfileImageToProfileImageLightModel);
        }

        public ProfileImageModel GetById(int? id)
        {
            var foundEntity = dbContextFactory
                .CreateDbContext()
                .ProfileImages
                .FirstOrDefault(t => t.Id == id);

            return foundEntity == null ? null : mapper.MapProfileImageToProfileImageModel(foundEntity);
        }

        public void Update(ProfileImageModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapProfileImageModelToProfileImage(model);
                dbContext.Update(entity);
                dbContext.SaveChanges();
            }
        }

        public ProfileImageModel Add(ProfileImageModel model)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var entity = mapper.MapProfileImageModelToProfileImage(model);
                var entry = dbContext.Entry(entity);
                entry.State = EntityState.Added;
                dbContext.SaveChanges();
                return mapper.MapProfileImageToProfileImageModel(entity);
            }
        }

        public void Remove(int? id)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var profileImage = new ProfileImage()
                {
                    Id = id
                };
                dbContext.ProfileImages.Attach(profileImage);
                dbContext.ProfileImages.Remove(profileImage);
                dbContext.SaveChanges();
            }
        }
    }

    public class ProfileImageLightRepository
    {
    }
}