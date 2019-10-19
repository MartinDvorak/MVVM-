using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Extensions;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class TeamsNewestPostsListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;

        public int PostsQuantity { get; set; } = 2;
        public int CommentsQuantity { get; set; } = 1;

        private UserLightModel loggedUser { get; set; }

        private TeamModel loadedTeam { get; set; } = null;

        public bool Visibility { get; set; } = false;

        public bool AddCommentDialogVisible { get; set; } = false;

        public ObservableCollection<PostModel> NewestPosts { get; set; } = new ObservableCollection<PostModel>();

        public TeamsNewestPostsListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            mediator.Register<UserAuthenticatedMessage>(LoadLoggedUser);

            //visibility and value loading handling
            mediator.Register<ShowTeamPostsToggleVisibilityMessage>(Load);
            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamInformationToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamMembersToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<AddPostMessage>(TurnVisibilityToFalse);
            mediator.Register<CreateNewTeamMessage>(TurnVisibilityToFalse);

            mediator.Register<SearchWindowClosedMessage>(SearchWindowClosedReload);

            ShowMorePostsTeamCommand = new RelayCommand(ShowMorePosts);
            ShowLessPostsTeamCommand = new RelayCommand(ShowLessPosts);
            ShowMoreCommentsTeamCommand = new RelayCommand(ShowMoreComments);
            ShowLessCommentsTeamCommand = new RelayCommand(ShowLessComments);

            AddPostCommand = new RelayCommand(AddPost, CanAddPost);
            AddCommentCommand = new RelayCommand<int?>(AddComment, CanAddComment);
            mediator.Register<CommentAddedMessage>(CommentAddedReload);
            mediator.Register<CloseCommentWindowTeamsMessage>(CloseCommentWindowHandle);

            DeletePostCommand = new RelayCommand<int?>(DeletePost, CanDeletePost);
        }

        private void LoadLoggedUser(UserAuthenticatedMessage userAuthenticatedMessage)
        {
            loggedUser = userAuthenticatedMessage.User;
        }


        private void TurnVisibilityToFalse(IMessage obj)
        {
            this.Visibility = false;
            PostsQuantity = 2;
            CommentsQuantity = 1;
        }

        public void Load(ShowTeamPostsToggleVisibilityMessage message)
        {
            if (Visibility == false)
            {
                NewestPosts.Clear();

                //load new posts when updated
                var teamLight = facade.ConvertTeamModelToTeamLightModel(message.TeamModel);
                loadedTeam = facade.GetDetail(teamLight) as TeamModel;

                var newestPosts = facade.FindNewestNPostsInTeam(loadedTeam, PostsQuantity)
                    .ToList();
                NewestPosts.AddRange(newestPosts);

                //sort comments
                foreach (var post in NewestPosts)
                {
                    post.Comments = post.Comments.OrderBy(c => c.Date).ToArray();
                    post.Comments = post.Comments.Take(CommentsQuantity).ToArray();
                }

                loadedTeam = message.TeamModel;
            }
            else
            {
                loadedTeam = new TeamModel();
                PostsQuantity = 2;
                CommentsQuantity = 1;
            }

            Visibility = !Visibility;
        }

        public override void Load()
        {
            NewestPosts.Clear();

            if (loadedTeam != null)
            {
                //load new posts when updated
                var teamLight = facade.ConvertTeamModelToTeamLightModel(loadedTeam);
                loadedTeam = facade.GetDetail(teamLight) as TeamModel;

                var newestPosts = facade.FindNewestNPostsInTeam(loadedTeam, PostsQuantity)
                    .ToList();
                NewestPosts.AddRange(newestPosts);

                //sort comments
                foreach (var post in NewestPosts)
                {
                    post.Comments = post.Comments.OrderBy(c => c.Date).ToArray();
                    post.Comments = post.Comments.Take(CommentsQuantity).ToArray();
                }
            }
        }




        public ICommand ShowMorePostsTeamCommand { get; }
        public ICommand ShowLessPostsTeamCommand { get; }
        public ICommand ShowMoreCommentsTeamCommand { get; }
        public ICommand ShowLessCommentsTeamCommand { get; }

        private void ShowMorePosts()
        {
            PostsQuantity += 2;
            Load();
        }
 
        private void ShowLessPosts()
        {
            if (PostsQuantity > 1)
            {
                PostsQuantity -= 2;
                Load();
            }
        }

        private void ShowMoreComments()
        {
            CommentsQuantity += 1;
            Load();
        }

        private void ShowLessComments()
        {
            if (CommentsQuantity > 0)
            {
                CommentsQuantity -= 1;
                Load();
            }
        }

        private void CommentAddedReload(CommentAddedMessage commentAddedMessage)
        {
            AddCommentDialogVisible = false;
            Load();
        }

        private void SearchWindowClosedReload(SearchWindowClosedMessage searchWindowClosedMessage)
        {
            Visibility = true;
            Load();
        }

        public ICommand AddPostCommand { get; set; }

        private Boolean CanAddPost() =>
            loadedTeam != null;

        public void AddPost() => mediator.Send(new AddPostMessage{TeamModel = loadedTeam});
      
        public ICommand AddCommentCommand { get; set; }

        private Boolean CanAddComment(int? id) =>
            id != null
            && !AddCommentDialogVisible;

        public void AddComment(int? id)
        {
            var lightModel = new PostLightModel
            {
                Id = id
            };

            PostModel model = facade.GetDetail(lightModel) as PostModel;
            mediator.Send(new AddCommentTeamsMessage{PostModel = model});

            AddCommentDialogVisible = true;
        }

        private void CloseCommentWindowHandle(CloseCommentWindowTeamsMessage closeCommentWindowTeamsMessage)
        {
            AddCommentDialogVisible = false;
        }


        public ICommand DeletePostCommand { get; set; }

        private Boolean CanDeletePost(int? id)
        {
            if (id != null)
                if (facade.GetDetail(new PostLightModel { Id = id }) as PostModel != null)
                    if (loggedUser.Id == (facade.GetDetail(new PostLightModel { Id = id }) as PostModel).Author.Id)
                        return true;
            return false;
        }

        private void DeletePost(int? id)
        {
            facade.Delete(new PostLightModel { Id = id });
            Load();
            mediator.Send(new PostOrCommentDeletedMessage());
        }
    }
}
