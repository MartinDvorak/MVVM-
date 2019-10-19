using System.Collections.Generic;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;

namespace TeamsManager.BL.Model
{
    public class UserModel : BaseModel.Model
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserDescription { get; set; }

        public ProfileImageLightModel ProfilePicture { get; set; }
        public ICollection<ContributionLightModel> MyContributions { get; set; }
        public ICollection<UserTeamMemberModel> UserTeams { get; set; }
        public ICollection<ContributionUserTagModel> ContributionUserTags { get; set; }
        public ICollection<TeamLightModel> AdministratedTeams { get; set; }

    }
}