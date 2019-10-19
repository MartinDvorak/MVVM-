using TeamsManager.BL.Model.BaseModel;

namespace TeamsManager.BL.Facade
{
    public interface IBasicDataManagement
    {
        void Delete(IModel model);
        void Update(IModel model);
        IModel Create(IModel model);
        IModel GetDetail(IModel model);
    }
}
