using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.DAL.Entities;
using TeamsManager.DAL.Entities.Contributions;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Mapper
{
    public interface IMapper
    {
        CommentModel MapCommentToCommentModel(Comment comment);
        Comment MapCommentModelToComment(CommentModel model);
        ContributionFile MapContributionFileModelToContributionFile(ContributionFileModel model);
        ContributionFileModel MapContributionFileToContributionFileModel(ContributionFile contributionFile);
        ContributionModel MapContributionToContributionModel(Contribution contribution);
        Contribution MapContributionModelToContribution(ContributionModel model);
        PostModel MapPostToPostModel(Post post);
        Post MapPostModelToPost(PostModel model);
        ProfileImageModel MapProfileImageToProfileImageModel(ProfileImage profileImage);
        ProfileImage MapProfileImageModelToProfileImage(ProfileImageModel model);
        TeamModel MapTeamToTeamModel(Team team);
        Team MapTeamModelToTeam(TeamModel model);
        UserModel MapUserToUserModel(User user);
        User MapUserModelToUser(UserModel model);
        ContributionUserTagModel MapContributionUserTagToContributionUserTagModel(ContributionUserTag userTag);
        ContributionUserTag MapContributionUserTagModelToContributionUserTag(ContributionUserTagModel model);
        UserTeamMemberModel MapUserTeamMemberToUserTeamMemberModel(UserTeamMember userTeamMember);
        UserTeamMember MapUserTeamMemberModelToUserTeamMember(UserTeamMemberModel model);
        CommentLightModel MapCommentToCommentLightModel(Comment comment);
        Comment MapCommentLightModelToComment(CommentLightModel lightModel);
        ContributionLightModel MapContributionToContributionLightModel(Contribution contribution);
        Contribution MapContributionLightModelToContribution(ContributionLightModel lightModel);
        PostLightModel MapPostToPostLightModel(Post post);
        Post MapPostLightModelToPost(PostLightModel lightModel);
        ContributionFileLightModel MapContributionFileToContributionFileLightModel(ContributionFile contributionFile);
        ContributionFile MapContributionFileLightModelToContributionFile(ContributionFileLightModel lightModel);
        ProfileImageLightModel MapProfileImageToProfileImageLightModel(ProfileImage profileImage);
        ProfileImage MapProfileImageLightModelToProfileImage(ProfileImageLightModel lightModel);
        TeamLightModel MapTeamToTeamLightModel(Team team);
        Team MapTeamLightModelToTeam(TeamLightModel lightModel);
        UserLightModel MapUserToUserLightModel(User user);
        User MapUserLightModelToUser(UserLightModel lightModel);


    }
}
