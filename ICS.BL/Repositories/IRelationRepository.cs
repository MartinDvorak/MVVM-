using System.Collections.Generic;

namespace TeamsManager.BL.Repositories
{
    public interface IRelationRepository<TModel>
        where TModel : class
    {
        IEnumerable<TModel> GetAll();
        TModel GetById(int? userId, int? relatedId);
        void Update(TModel model);
        TModel Add(TModel model);
        void Remove(int? userId, int? relatedId);
    }
}
