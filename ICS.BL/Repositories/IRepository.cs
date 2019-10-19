using System.Collections.Generic;
using TeamsManager.BL.Mapper;
using TeamsManager.DAL;

namespace TeamsManager.BL.Repositories
{
    public interface IRepository<TModel, out TLightModel> 
        where TModel : class
        where TLightModel : class
    {
        IEnumerable<TLightModel> GetAll();
        TModel GetById(int? id);
        void Update(TModel model);
        TModel Add(TModel model);
        void Remove(int? id);
    }
}
