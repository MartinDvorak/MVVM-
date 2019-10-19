using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.Internal;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Extensions;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class UsersNewestPostsListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;

        private UserLightModel loggedUser { get; set; } = null;

        public bool AddCommentDialogVisible { get; set; } = false;
        public bool visible { get; set; } = true;
        public int PostsQuantity { get; set; } = 2;
        public int CommentsQuantity { get; set; } = 1;
        public ObservableCollection<PostModel> NewestPosts { get; set; } = new ObservableCollection<PostModel>();

        public UsersNewestPostsListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            ShowMorePostsUserCommand = new RelayCommand(ShowMorePosts);
            ShowLessPostsUserCommand = new RelayCommand(ShowLessPosts);
            ShowMoreCommentsUserCommand = new RelayCommand(ShowMoreComments);
            ShowLessCommentsUserCommand = new RelayCommand(ShowLessComments);

            mediator.Register<UserDetailToggleVisibilityMessage>(ToggleVisibility);
            mediator.Register<UserAuthenticatedMessage>(Load);

            AddCommentCommand = new RelayCommand<int?>(AddComment, CanAddComment);
            mediator.Register<CommentAddedMessage>(CommentAddedReload);
            mediator.Register<CloseCommentWindowUsersMessage>(CloseCommentWindowHandle);

            DeletePostCommand = new RelayCommand<int?>(DeletePost, CanDeletePost);
        }

        public void Load(UserAuthenticatedMessage loggedInMessage)
        {
            NewestPosts.Clear();

            var newestPosts = facade.FindNewestNPostsForUser(loggedInMessage.User, PostsQuantity)
                .ToList();
            NewestPosts.AddRange(newestPosts);

            //sort comments
            foreach (var post in NewestPosts)
            {
                post.Comments = post.Comments.OrderBy(c => c.Date).ToArray();
                post.Comments = post.Comments.Take(CommentsQuantity).ToArray();
            }

            loggedUser = loggedInMessage.User;            
        }

        public override void Load()
        {
            if (loggedUser != null)
            {
                NewestPosts.Clear();

                var newestPosts = facade.FindNewestNPostsForUser(loggedUser, PostsQuantity)
                    .ToList();
                NewestPosts.AddRange(newestPosts);

                foreach (var post in NewestPosts)
                {
                    post.Comments = post.Comments.OrderBy(c => c.Date).ToArray();
                    post.Comments = post.Comments.Take(CommentsQuantity).ToArray();
                }
            }
        }

        private void CommentAddedReload(CommentAddedMessage commentAddedMessage)
        {
            AddCommentDialogVisible = false;
            Load();
        }

        private void ToggleVisibility(UserDetailToggleVisibilityMessage userDetailToggleVisibilityMessage)
        {
            visible = !visible;
            if (!visible)
            {
                PostsQuantity = 2;
                CommentsQuantity = 1;
            }
        }

        public ICommand ShowMorePostsUserCommand { get; }
        public ICommand ShowMoreCommentsUserCommand { get; }

        public ICommand ShowLessPostsUserCommand { get; }
        public ICommand ShowLessCommentsUserCommand { get; }



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
            mediator.Send(new AddCommentUsersMessage { PostModel = model });

            AddCommentDialogVisible = true;
        }

        private void CloseCommentWindowHandle(CloseCommentWindowUsersMessage closeCommentWindowTeamsMessage)
        {
            AddCommentDialogVisible = false;
        }


        public ICommand DeletePostCommand { get; set; }

        private Boolean CanDeletePost(int? id)
        {
            if(id != null)
                if(facade.GetDetail(new PostLightModel { Id = id }) as PostModel != null)
                    if (loggedUser.Id == (facade.GetDetail(new PostLightModel {Id = id}) as PostModel).Author.Id)
                        return true;
            return false;
        }

        private void DeletePost(int? id)
        {
            facade.Delete(new PostLightModel{Id = id});
            Load();
            mediator.Send(new PostOrCommentDeletedMessage());
        }
    }
}
