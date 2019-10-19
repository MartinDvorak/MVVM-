using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging.Abstractions;
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
    public class Mapper : IMapper
    {
        public CommentModel MapCommentToCommentModel(Comment comment)
        {
            if (comment == null)            
                return null;

            return new CommentModel
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                Author = MapUserToUserLightModel(comment.Author),
                AssociatedFiles = MapCollection(comment.AssociatedFiles, MapContributionFileToContributionFileLightModel),
                ContributionUserTags = MapCollection(comment.ContributionUserTags, MapContributionUserTagToContributionUserTagModel),
                ParentContribution = MapPostToPostLightModel(comment.ParentContribution)
            };
        }

        public Comment MapCommentModelToComment(CommentModel model)
        {
            if (model == null)
                return null;

            return new Comment
            {
                Id = model.Id,
                Content = model.Content,
                Date = model.Date,
                Author = MapUserLightModelToUser(model.Author),
                AssociatedFiles = MapCollection(model.AssociatedFiles, MapContributionFileLightModelToContributionFile),
                ContributionUserTags = MapCollection(model.ContributionUserTags, MapContributionUserTagModelToContributionUserTag),
                ParentContribution = MapPostLightModelToPost(model.ParentContribution)
            };
        }

        public ContributionFileModel MapContributionFileToContributionFileModel(ContributionFile contributionFile)
        {
            if (contributionFile == null)
                return null;

            return new ContributionFileModel
            {
                Id = contributionFile.Id,
                FileName = contributionFile.FileName,
                Content = contributionFile.Content,
                AssociatedContribution = MapContributionToContributionLightModel(contributionFile.AssociatedContribution),
                FileFormat = contributionFile.FileFormat
            };
        }

        public ContributionFile MapContributionFileModelToContributionFile(ContributionFileModel model)
        {
            if (model == null)
                return null;

            return new ContributionFile
            {
                Id = model.Id,
                FileName = model.FileName,
                Content = model.Content,
                AssociatedContribution = MapContributionLightModelToContribution(model.AssociatedContribution),
                FileFormat = model.FileFormat
            };
        }

        public ContributionModel MapContributionToContributionModel(Contribution contribution)
        {
            if (contribution == null)
                return null;

            switch (contribution)
            {
                case Post post:
                    return MapPostToPostModel(post);
                case Comment comment:
                    return MapCommentToCommentModel(comment);
                default:
                    throw new Exception("Unknown type of contribution");
            }
        }

        public Contribution MapContributionModelToContribution(ContributionModel model)
        {
            if (model == null)
                return null;

            switch (model)
            {
                case PostModel postModel:
                    return MapPostModelToPost(postModel);
                case CommentModel commentModel:
                    return MapCommentModelToComment(commentModel);
                default:
                    throw new Exception("Unknown type of contribution model");
            }
        }

        public PostModel MapPostToPostModel(Post post)
        {
            if (post == null)
                return null;

            return new PostModel
            {
                Id = post.Id,
                Content = post.Content,
                Date = post.Date,
                Author = MapUserToUserLightModel(post.Author),
                AssociatedFiles = MapCollection(post.AssociatedFiles, MapContributionFileToContributionFileLightModel),
                ContributionUserTags = MapCollection(post.ContributionUserTags, MapContributionUserTagToContributionUserTagModel),
                Title = post.Title,
                CorrespondingTeam = MapTeamToTeamLightModel(post.CorrespondingTeam),
                Comments = MapCollection(post.Comments, MapCommentToCommentModel),
            };
        }

        public Post MapPostModelToPost(PostModel model)
        {
            if (model == null)
                return null;

            return new Post
            {
                Id = model.Id,
                Content = model.Content,
                Date = model.Date,
                Author = MapUserLightModelToUser(model.Author),
                AssociatedFiles = MapCollection(model.AssociatedFiles, MapContributionFileLightModelToContributionFile),
                ContributionUserTags = MapCollection(model.ContributionUserTags, MapContributionUserTagModelToContributionUserTag),
                Title = model.Title,
                CorrespondingTeam = MapTeamLightModelToTeam(model.CorrespondingTeam),
                Comments = MapCollection(model.Comments, MapCommentModelToComment)
            };
        }

        public ProfileImageModel MapProfileImageToProfileImageModel(ProfileImage profileImage)
        {
            if (profileImage == null)
                return null;

            return new ProfileImageModel
            {
                Id = profileImage.Id,
                FileName = profileImage.FileName,
                Content = profileImage.Content,
                PictureFormat = profileImage.PictureFormat
            };
        }

        public ProfileImage MapProfileImageModelToProfileImage(ProfileImageModel model)
        {
            if (model == null)
                return null;

            return new ProfileImage
            {
                Id = model.Id,
                FileName = model.FileName,
                Content = model.Content,
                PictureFormat = model.PictureFormat
            };
        }

        public TeamModel MapTeamToTeamModel(Team team)
        {
            if (team == null)
                return null;

            return new TeamModel
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Admin = MapUserToUserLightModel(team.Admin),
                Members = MapCollection(team.TeamMembers, MapUserTeamMemberToUserTeamMemberModel),
                Posts = MapCollection(team.Posts, MapPostToPostLightModel)
            };
        }

        public Team MapTeamModelToTeam(TeamModel model)
        {
            if (model == null)
                return null;

            return new Team
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Admin = MapUserLightModelToUser(model.Admin),
                TeamMembers = MapCollection(model.Members, MapUserTeamMemberModelToUserTeamMember),
                Posts = MapCollection(model.Posts, MapPostLightModelToPost)
            };
        }

        public UserModel MapUserToUserModel(User user)
        {
            if (user == null)
                return null;

            return new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                UserDescription = user.UserDescription,
                ProfilePicture = MapProfileImageToProfileImageLightModel(user.Photo),
                MyContributions = MapCollection(user.MyContributions, MapContributionToContributionLightModel),
                AdministratedTeams = MapCollection(user.AdministratedTeams, MapTeamToTeamLightModel),
                UserTeams = MapCollection(user.UserTeams, MapUserTeamMemberToUserTeamMemberModel),
                ContributionUserTags = MapCollection(user.ContributionUserTags, MapContributionUserTagToContributionUserTagModel)
            };
            
        }

        public User MapUserModelToUser(UserModel model)
        {
            if (model == null)
                return null;

            return new User
            {
                Id = model.Id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
                UserDescription = model.UserDescription,
                Photo = MapProfileImageLightModelToProfileImage(model.ProfilePicture),
                MyContributions = MapCollection(model.MyContributions, MapContributionLightModelToContribution),
                AdministratedTeams = MapCollection(model.AdministratedTeams, MapTeamLightModelToTeam),
                UserTeams = MapCollection(model.UserTeams, MapUserTeamMemberModelToUserTeamMember),
                ContributionUserTags = MapCollection(model.ContributionUserTags, MapContributionUserTagModelToContributionUserTag)
            };
        }

        public ContributionUserTagModel MapContributionUserTagToContributionUserTagModel(ContributionUserTag taggedUser)
        {
            if (taggedUser == null)
                return null;

            return new ContributionUserTagModel
            {
                ContributionId = taggedUser.ContributionId,
                Contribution = MapContributionToContributionLightModel(taggedUser.Contribution),
                User = MapUserToUserLightModel(taggedUser.User),
                UserId = taggedUser.UserId
            };
        }

        public ContributionUserTag MapContributionUserTagModelToContributionUserTag(ContributionUserTagModel model)
        {
            if (model == null)
                return null;

            return new ContributionUserTag
            {
                ContributionId = model.ContributionId,
                Contribution = MapContributionLightModelToContribution(model.Contribution),
                User = MapUserLightModelToUser(model.User),
                UserId = model.UserId
            };
        }

        public UserTeamMemberModel MapUserTeamMemberToUserTeamMemberModel(UserTeamMember userTeamMember)
        {
            if (userTeamMember == null)
                return null;

            return new UserTeamMemberModel
            {
                Team = MapTeamToTeamLightModel(userTeamMember.Team),
                TeamId = userTeamMember.TeamId,
                User = MapUserToUserLightModel(userTeamMember.User),
                UserId = userTeamMember.UserId
            };
        }

        public UserTeamMember MapUserTeamMemberModelToUserTeamMember(UserTeamMemberModel model)
        {
            if (model == null)
                return null;

            return new UserTeamMember
            {
                Team = MapTeamLightModelToTeam(model.Team),
                TeamId = model.TeamId,
                User = MapUserLightModelToUser(model.User),
                UserId = model.UserId
            };
        }

        public CommentLightModel MapCommentToCommentLightModel(Comment comment)
        {
            if (comment == null)
                return null;

            return new CommentLightModel
            {
                Id = comment.Id,
                Content = comment.Content,
                Date = comment.Date,
                Author = MapUserToUserLightModel(comment.Author)
            };
        }

        public Comment MapCommentLightModelToComment(CommentLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new Comment
            {
                Id = lightModel.Id,
                Content = lightModel.Content,
                Date = lightModel.Date,
                Author = MapUserLightModelToUser(lightModel.Author)
            };
        }

        public ContributionLightModel MapContributionToContributionLightModel(Contribution contribution)
        {
            if (contribution == null)
                return null;

            switch (contribution)
            {
                case Post post:
                    return MapPostToPostLightModel(post);
                case Comment comment:
                    return MapCommentToCommentLightModel(comment);
                default:
                    throw new Exception("Unknown type of contribution");
            }
        }

        public Contribution MapContributionLightModelToContribution(ContributionLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            switch (lightModel)
            {
                case PostLightModel postLightModel:
                    return MapPostLightModelToPost(postLightModel);
                case CommentLightModel commentLightModel:
                    return MapCommentLightModelToComment(commentLightModel);
                default:
                    throw new Exception("Unknown type of contribution light model");
            }
        }

        public PostLightModel MapPostToPostLightModel(Post post)
        {
            if (post == null)
                return null;

            return new PostLightModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Date = post.Date,
                Comments = MapCollection(post.Comments, MapCommentToCommentLightModel)
            };
        }

        public Post MapPostLightModelToPost(PostLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new Post
            {
                Id = lightModel.Id,
                Title = lightModel.Title,
                Content = lightModel.Content,
                Date = lightModel.Date,
                Comments = MapCollection(lightModel.Comments, MapCommentLightModelToComment)
            };
        }

        public ContributionFileLightModel MapContributionFileToContributionFileLightModel(ContributionFile contributionFile)
        {
            if (contributionFile == null)
                return null;

            return new ContributionFileLightModel
            {
                Id = contributionFile.Id,
                FileName = contributionFile.FileName
            };
        }

        public ContributionFile MapContributionFileLightModelToContributionFile(ContributionFileLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new ContributionFile
            {
                Id = lightModel.Id,
                FileName = lightModel.FileName
            };
        }

        public ProfileImageLightModel MapProfileImageToProfileImageLightModel(ProfileImage profileImage)
        {
            if (profileImage == null)
                return null;

            return new ProfileImageLightModel
            {
                Id = profileImage.Id,
                PictureFormat = profileImage.PictureFormat,
                Content = profileImage.Content
            };
        }

        public ProfileImage MapProfileImageLightModelToProfileImage(ProfileImageLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new ProfileImage
            {
                Id = lightModel.Id,
                PictureFormat = lightModel.PictureFormat,
                Content = lightModel.Content
            };
        }

        public TeamLightModel MapTeamToTeamLightModel(Team team)
        {
            if (team == null)
                return null;

            return new TeamLightModel
            {
                Id = team.Id,
                Name = team.Name
            };
        }

        public Team MapTeamLightModelToTeam(TeamLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new Team
            {
                Id = lightModel.Id,
                Name = lightModel.Name
            };
        }

        public UserLightModel MapUserToUserLightModel(User user)
        {
            if (user == null)
                return null;

            return new UserLightModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserDescription = user.UserDescription,
                ProfilePicture = MapProfileImageToProfileImageLightModel(user.Photo)
            };
        }

        public User MapUserLightModelToUser(UserLightModel lightModel)
        {
            if (lightModel == null)
                return null;

            return new User
            {
                Id = lightModel.Id,
                FirstName = lightModel.FirstName,
                LastName = lightModel.LastName,
                Email = lightModel.Email,
                UserDescription = lightModel.UserDescription,
                Photo = MapProfileImageLightModelToProfileImage(lightModel.ProfilePicture)
            };
        }

        private static Collection<T2> MapCollection<T1, T2>(IEnumerable<T1> mappedCollection, Func<T1, T2> mappingFunction)
        {
            var targetCollection = new Collection<T2>();

            if (mappedCollection == null)
                return targetCollection;

            foreach (var item in mappedCollection)
            {
                targetCollection.Add(mappingFunction(item));
            }

            return targetCollection;
        }

        

    }
}
