using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Remotion.Linq.Clauses;
using TeamsManager.BL.Mapper;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.BaseModel;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Repositories;
using TeamsManager.DAL.DbContext;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.BL.Facade
{
    public class BusinessLogicFacade : IBusinessLogicFacade
    {
        private readonly IRepository<CommentModel,CommentLightModel> commentRepository;
        private readonly IRepository<ContributionFileModel,ContributionFileLightModel> contributionFileRepository;
        private readonly IRepository<PostModel,PostLightModel> postRepository;
        private readonly IRepository<ProfileImageModel,ProfileImageLightModel> profileImageRepository;
        private readonly IRepository<TeamModel,TeamLightModel> teamRepository;
        private readonly IRepository<UserModel,UserLightModel> userRepository;
        private readonly IRelationRepository<ContributionUserTagModel> contributionUserTagRepository;
        private readonly IRelationRepository<UserTeamMemberModel> userTeamMemberRepository;

        public BusinessLogicFacade(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            this.commentRepository = new CommentRepository(dbContextFactory, mapper);
            this.contributionFileRepository = new ContributionFileRepository(dbContextFactory, mapper);
            this.postRepository = new PostRepository(dbContextFactory, mapper);
            this.profileImageRepository = new ProfileImageRepository(dbContextFactory, mapper);
            this.teamRepository = new TeamRepository(dbContextFactory, mapper);
            this.userRepository = new UserRepository(dbContextFactory, mapper);
            this.contributionUserTagRepository = new ContributionUserTagRepository(dbContextFactory,mapper);
            this.userTeamMemberRepository = new UserTeamMemberRepository(dbContextFactory,mapper);
        }
        public void Delete(IModel model)
        {
            switch (model)
            {
                case TeamLightModel _:
                case TeamModel _:
                    this.teamRepository.Remove(model.Id);
                    break;
                case CommentLightModel _:
                case CommentModel _:
                    this.commentRepository.Remove(model.Id);
                    break;
                case PostLightModel _:
                case PostModel _:
                    RemoveComments(model.Id);
                    this.postRepository.Remove(model.Id);
                    break;
                case UserLightModel _:
                case UserModel _:
                    this.userRepository.Remove(model.Id);
                    break;
                case ProfileImageLightModel _:
                case ProfileImageModel _:
                    this.profileImageRepository.Remove(model.Id);
                    break;
                case ContributionFileLightModel _:
                case ContributionFileModel _:
                    this.contributionFileRepository.Remove(model.Id);
                    break;
            }
        }

        private void RemoveComments(int? id)
        {
            PostModel model = GetDetail(new PostLightModel {Id = id}) as PostModel;

            foreach (CommentModel commentModel in model.Comments)
            {
                this.commentRepository.Remove(commentModel.Id);
            }
        }

        public void Update(IModel model)
        {
            switch (model)
            {
                case CommentModel commentModel:
                    this.commentRepository.Update(commentModel);
                    break;
                case PostModel postModel:
                    this.postRepository.Update(postModel);
                    break;
                case UserModel userModel:
                    this.userRepository.Update(userModel);
                    break;
                case TeamModel teamModel:
                    this.teamRepository.Update(teamModel);
                    break;
                case ProfileImageModel profileImageModel:
                    this.profileImageRepository.Update(profileImageModel);
                    break;
                case ContributionFileModel contributionFileModel:
                    this.contributionFileRepository.Update(contributionFileModel);
                    break;
            }
        }

        public IModel Create(IModel model)
        {
            switch (model)
            {
                case CommentModel commentModel:
                    return this.commentRepository.Add(commentModel);
                case PostModel postModel:
                    return this.postRepository.Add(postModel);
                case UserModel userModel:
                    userModel.ProfilePicture = SetDefaultAvatar();
                    return this.userRepository.Add(userModel);
                case TeamModel teamModel:
                    return this.teamRepository.Add(teamModel);
                case ProfileImageModel profileImageModel:
                    return this.profileImageRepository.Add(profileImageModel);
                case ContributionFileModel contributionFileModel:
                    return this.contributionFileRepository.Add(contributionFileModel);
                default:
                    return null;
            }
        }

        private ProfileImageLightModel SetDefaultAvatar()
        {
            var avatar = System.Drawing.Image.FromFile(@"DefaultImages\defaultAvatar.png");
            return new ProfileImageLightModel()
            {
                Content = ConvertImageToByteArray(avatar),
                PictureFormat = SupportedFormatPicture.Png
            };
        }

        public IModel GetDetail(IModel model)
        {
            switch (model)
            {
                case CommentLightModel commentLightModel:
                    return this.commentRepository.GetById(commentLightModel.Id);
                case PostLightModel postLightModel:
                    return this.postRepository.GetById(postLightModel.Id);
                case UserLightModel userLightModel:
                    return this.userRepository.GetById(userLightModel.Id);
                case TeamLightModel teamLightModel:
                    return this.teamRepository.GetById(teamLightModel.Id);
                case ProfileImageLightModel profileImageLightModel:
                    return this.profileImageRepository.GetById(profileImageLightModel.Id);
                case ContributionFileLightModel contributionFileLightModel:
                    return this.contributionFileRepository.GetById(contributionFileLightModel.Id);
                default:
                    return model;
            }
        }

        public UserLightModel FindUserByEmail(string email)
        {
            var allUsers = this.userRepository.GetAll().ToList();
            return allUsers.FirstOrDefault(user => user.Email == email);
        }

        public string HashPassword(string password)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                var builder = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool IsEmailValid(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool AreUserDataValid(string firstName, string lastName, string email, string passwordHash,
            string passwordConfirmationHash)
        {
            var isFirstNameValid = !string.IsNullOrEmpty(firstName);
            var isLastNameValid = !string.IsNullOrEmpty(lastName);
            var isEmailValid = IsEmailValid(email);
            var isPasswordValid = passwordHash.Equals(passwordConfirmationHash);

            return isFirstNameValid && isLastNameValid && isEmailValid && isPasswordValid;
        }

        public byte[] ConvertImageToByteArray(Image image)
        {
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public SupportedFormatPicture ConvertFileExtenstionToEnum(string fileExtension)
        {
            SupportedFormatPicture selectedFormat;
            switch (fileExtension)
            {
                case "png":
                    selectedFormat = SupportedFormatPicture.Png;
                    break;
                case "jpeg":
                    selectedFormat = SupportedFormatPicture.Jpg;
                    break;
                case "gif":
                    selectedFormat = SupportedFormatPicture.Gif;
                    break;
                default:
                    selectedFormat = SupportedFormatPicture.Jpg;
                    break;
            }
            return selectedFormat;
        }

        public IEnumerable<TeamLightModel> FindMyTeams(UserLightModel user)
        {
            var allMembership = this.userTeamMemberRepository.GetAll().ToList();
            var allTeams = this.teamRepository.GetAll().ToList();
            return findMyTeams(allMembership,allTeams,user);
        }

        private IEnumerable<TeamLightModel> findMyTeams(IEnumerable<UserTeamMemberModel> allMembership,
            IEnumerable<TeamLightModel> allTeams, UserLightModel user)
        {
            var myTeamsIds = (from membership in allMembership where membership.UserId == user.Id select membership.TeamId).ToList();
            var myTeams = new List<TeamLightModel>();
            foreach (var myTeamId in myTeamsIds)
            {
                myTeams.Add(allTeams.FirstOrDefault(team => team.Id == myTeamId));
            }
            return myTeams;
        }


        public IEnumerable<UserLightModel> GetAllMembers(TeamLightModel team)
        {
            var allMembership = this.userTeamMemberRepository.GetAll().ToList();
            var allUsers = this.userRepository.GetAll().ToList();
            return findMyMembers(allMembership,allUsers,team);
        }

        private IEnumerable<UserLightModel> findMyMembers(IEnumerable<UserTeamMemberModel> allMembership,
            IEnumerable<UserLightModel> allUsers, TeamLightModel team)
        {
            var myUsersIds = (from membership in allMembership where membership.TeamId == team.Id select membership.UserId).ToList();
            var myUsers = new List<UserLightModel>();
            foreach (var myUserId in myUsersIds)
            {
                myUsers.Add(allUsers.FirstOrDefault(user => myUserId == user.Id));
            }
            return myUsers;
        }

        public void JoinUserToTeam(UserLightModel user, TeamLightModel team)
        {
            var requestingUser = new UserTeamMemberModel
            {
                Team = team,
                TeamId = team.Id,
                User = user,
                UserId = user.Id
            };
            this.userTeamMemberRepository.Add(requestingUser);         
        }

        public void DeleteUserFromTeam(UserLightModel user, TeamLightModel team)
        {
            var unWantedUser = new UserTeamMemberModel
            {
                Team = team,
                TeamId = team.Id,
                User = user,
                UserId = user.Id
            };
            this.userTeamMemberRepository.Remove(unWantedUser.UserId,unWantedUser.TeamId);
        }

        public IEnumerable<UserLightModel> GetAllNonMembers(TeamLightModel team)
        {
            var allUsers = this.userRepository.GetAll().ToList();
            var membersIds = getMembersIds(team);
            var nonMembers = new List<UserLightModel>();
            foreach (var user in allUsers)
            {
                if (!isUserInTeam(user, membersIds))
                    nonMembers.Add(user);
            }
            return nonMembers;
        }

        private bool isUserInTeam(UserLightModel user, IEnumerable<int?> membersIds)
        {
            return membersIds.Any(memberId => memberId == user.Id);
        }

        private IEnumerable<int?> getMembersIds(TeamLightModel team)
        {
            var membersIds = new List<int?>();
            var detailTeam = GetDetail(team) as TeamModel;
            foreach (var membership in detailTeam.Members)
            {
                membersIds.Add(membership.UserId);
            }
            return membersIds;
        }

        public void DelegateAdminPosition(UserLightModel user, TeamLightModel team)
        {
            var teamModel = GetDetail(team) as TeamModel;
            var teamWithNewAdmin = new TeamModel
            {
                Admin = user,
                Description = teamModel.Description,
                Id = teamModel.Id,
                Members = teamModel.Members,
                Name = teamModel.Name,
                Posts = teamModel.Posts
            };
            this.Update(teamWithNewAdmin);
        }

        public bool IsUserAdminInTeam(UserLightModel user, TeamLightModel team)
        {
            var teamDetail = GetDetail(team) as TeamModel;
            return teamDetail.Admin.Id == user.Id;
        }

        private IEnumerable<PostModel> selectNewestNPost(IEnumerable<Tuple<DateTime, PostLightModel>> allPosts,int n)
        {
            var nNewestLightPostsTuples = allPosts.OrderByDescending((tuple => tuple.Item1)).Take(n);
            var nNewestLightPosts = takeOnlyLightPosts(nNewestLightPostsTuples);
            return convertLightModelsToFullModels(nNewestLightPosts).Cast<PostModel>();
        }

        public IEnumerable<PostModel> FindNewestNPostsForUser(UserLightModel user, int n)
        {
            var myTeams = convertLightModelsToFullModels(this.FindMyTeams(user)).Cast<TeamModel>();
            var timeOfNewestCommentInPost = new List<Tuple<DateTime, PostLightModel>>();
            foreach (var team in myTeams)
            {
                foreach (var post in team.Posts)
                {
                    timeOfNewestCommentInPost.Add(findNewesetCommentTimeInPost(post));
                }
            }
            return selectNewestNPost(timeOfNewestCommentInPost,n);
        }

        public IEnumerable<PostModel> FindNewestNPostsInTeam(TeamModel team, int n)
        {
            var timeOfNewestCommentInPost = new List<Tuple<DateTime, PostLightModel>>();

            foreach (var post in team.Posts)
            {
                timeOfNewestCommentInPost.Add(findNewesetCommentTimeInPost(post));
            }
            return selectNewestNPost(timeOfNewestCommentInPost, n);
        }

        private Tuple<DateTime, PostLightModel> findNewesetCommentTimeInPost(PostLightModel post)
        {
            var newestComment = findNewestCommentInPost(post.Comments);
            if (newestComment == null)
                newestComment = post.Date;
            return Tuple.Create( (DateTime) newestComment, post);
        }

        private IEnumerable<PostLightModel> takeOnlyLightPosts(IEnumerable<Tuple<DateTime, PostLightModel>> nNewestLightPostsTuples)
        {
            var posts = new List<PostLightModel>();
            foreach (var tuple in nNewestLightPostsTuples)
            {
                posts.Add(tuple.Item2);
            }
            return posts;
        }


        private DateTime? findNewestCommentInPost(ICollection<CommentLightModel> postComments)
        {
            var newestComment = postComments.OrderByDescending(comment => comment.Date).FirstOrDefault();
            if (newestComment == null)
                return null;
            return newestComment.Date;
        }


        public IEnumerable<CommentModel> FindNewestNCommentsInPost(PostModel post, int n)
        {
            var sortedLightComments = post.Comments.OrderByDescending(comment=> comment.Date);
            return convertLightModelsToFullModels(sortedLightComments.Take(n)).Cast<CommentModel>();
        }

        private IEnumerable<IModel> convertLightModelsToFullModels(IEnumerable<IModel> lightModels)
        {
            var fullModels = new List<IModel>();
            foreach (var lightModel in lightModels)
            {
                fullModels.Add(this.GetDetail(lightModel));
            }
            return fullModels;
        }

        public IEnumerable<ContributionLightModel> FindNewestNTagsOfUser(UserLightModel user, int n)
        {
            var allTags = this.contributionUserTagRepository.GetAll().ToList();
            return (from tag in allTags where tag.UserId == user.Id select tag.Contribution);
        }
        
        public IEnumerable<PostModel> GetAllPostsInTeam(TeamModel team)
        {
            return convertLightModelsToFullModels(team.Posts).Cast<PostModel>();
        }

        public IEnumerable<CommentModel> GetAllCommentInPost(PostModel post)
        {
            return convertLightModelsToFullModels(post.Comments).Cast<CommentModel>();
        }

        public IEnumerable<ContributionLightModel> Search(string pattern, TeamModel team)
        {
            var results = new List<ContributionLightModel>();
            foreach (var post in team.Posts)
            {
                results = apllyPattern(pattern, post, results);
                foreach (var comment in post.Comments)
                {
                    results = apllyPattern(pattern, comment, results);
                }
            }
            return results;
        }

        private List<ContributionLightModel> apllyPattern(string pattern, ContributionLightModel contribution, List<ContributionLightModel> results)
        {
            switch (contribution)
            {
                case CommentLightModel commentModel:
                    if (isMatchFound(pattern, commentModel.Content))
                        results.Add(contribution);
                    break;
                case PostLightModel postModel:
                    if (isMatchFound(pattern, postModel.Title))
                        results.Add(contribution);
                    else if (isMatchFound(pattern, postModel.Content))
                        results.Add(contribution);
                    break;
                default:
                    return results;
            }
            return results;
        }

        private bool isMatchFound(string pattern, string text)
        {
            return text.Contains(pattern);
        }

        public IEnumerable<ContributionLightModel> Search(string pattern, UserLightModel user)
        {
            IEnumerable<TeamModel> userTeams = new List<TeamModel>();
            userTeams = convertLightModelsToFullModels(this.FindMyTeams(user)).Cast<TeamModel>();
            var results = new List<ContributionLightModel>();
            foreach (var team in userTeams)
            {
                results.Concat(this.Search(pattern, team));
            }
            return results;
        }

        public IEnumerable<MyRecentActivityModel> GetRecentUserActivity(UserLightModel user)
        {
            const int contributionViewedNumber = 5;
            var detailUser = GetDetail(user) as UserModel;
            List<MyRecentActivityModel> myRecentActivity = new List<MyRecentActivityModel>();
            foreach (var contribution in detailUser.MyContributions.OrderByDescending(contribution => contribution.Date).Take(contributionViewedNumber))
            {
                TeamLightModel teamAssociatedWithContribution = new TeamLightModel();
                switch (contribution)
                {
                    case PostLightModel post:
                        teamAssociatedWithContribution = findAssociatedTeam(post);
                        break;
                    case CommentLightModel comment:
                        teamAssociatedWithContribution = findAssociatedTeam(findAssociatedPost(comment));
                        break;
                }
                myRecentActivity.Add(new MyRecentActivityModel
                {
                    PublicationTime = contribution.Date, Contribution = contribution,
                    Team = teamAssociatedWithContribution
                });
            }
            return myRecentActivity;
        }

        public void SetTaggedUserInPost(IEnumerable<UserLightModel> users, ContributionModel contribution)
        {
            foreach (var user in users)
            {
                var userTaggModel = new ContributionUserTagModel
                {
                    ContributionId = contribution.Id,
                    UserId = user.Id,
                    User = user
                };
                this.contributionUserTagRepository.Add(userTaggModel);
            }
        }

        private PostLightModel findAssociatedPost(CommentLightModel comment)
        {
            return ((CommentModel) GetDetail(comment)).ParentContribution;
        }

        private TeamLightModel findAssociatedTeam(PostLightModel post)
        {
            return ((PostModel) GetDetail(post)).CorrespondingTeam;
        }

        public TeamLightModel ConvertTeamModelToTeamLightModel(TeamModel fullModel)
        {
            return new TeamLightModel
            {
                Id = fullModel.Id,
                Name = fullModel.Name
            };
        }
    }
}
