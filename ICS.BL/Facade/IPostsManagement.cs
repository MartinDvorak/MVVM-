using System.Collections.Generic;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;

namespace TeamsManager.BL.Facade
{
    public interface IPostsManagement
    {
        IEnumerable<PostModel> FindNewestNPostsForUser(UserLightModel user, int n);
        IEnumerable<PostModel> FindNewestNPostsInTeam(TeamModel team, int n);
        IEnumerable<CommentModel> FindNewestNCommentsInPost(PostModel post, int n);
        IEnumerable<ContributionLightModel> FindNewestNTagsOfUser(UserLightModel user, int n);
        IEnumerable<PostModel> GetAllPostsInTeam(TeamModel team);
        IEnumerable<CommentModel> GetAllCommentInPost(PostModel post);
        IEnumerable<ContributionLightModel> Search(string pattern, TeamModel team);
        IEnumerable<ContributionLightModel> Search(string pattern, UserLightModel user);
        IEnumerable<MyRecentActivityModel> GetRecentUserActivity(UserLightModel user);
        void SetTaggedUserInPost(IEnumerable<UserLightModel> users, ContributionModel contribution);
    }
}
