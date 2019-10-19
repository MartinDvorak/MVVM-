
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Facade
{
    public interface IBusinessLogicFacade : IBasicDataManagement, IPostsManagement, ITeamsAdministration, ITeamsManagement, IUserManagement
    {
    }
}
