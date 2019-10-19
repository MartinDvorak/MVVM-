using System.Collections.ObjectModel;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class UsersTagsListViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;


        public int contributionsQuantity { get; set; } = 3;

        private UserLightModel loggedUser { get; set; }

        public bool visible { get; set; } = true;
        public ObservableCollection<PostLightModel> TaggedInPosts { get; set; } = new ObservableCollection<PostLightModel>();
        public ObservableCollection<CommentLightModel> TaggedInComments { get; set; } = new ObservableCollection<CommentLightModel>();

        public UsersTagsListViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.mediator = mediator;
            this.facade = facade;

            TaggedInPostSelectedCommand = new RelayCommand<PostLightModel>(TaggedInPostSelected);
            TaggedInCommentSelectedCommand = new RelayCommand<CommentLightModel>(TaggedInCommentSelected);

            mediator.Register<UserDetailToggleVisibilityMessage>(ToggleVisibility);
            mediator.Register<UserAuthenticatedMessage>(Load);
            mediator.Register<CommentAddedMessage>(Load);
            mediator.Register<PostAddedMessage>(Load);
            mediator.Register<PostOrCommentDeletedMessage>(Load);
        }

        public void Load(UserAuthenticatedMessage loggedInMessage)
        {
            loggedUser = loggedInMessage.User;

            TaggedInPosts.Clear();
            TaggedInComments.Clear();

            var taggedInContributions = facade.FindNewestNTagsOfUser(loggedInMessage.User, contributionsQuantity);
            foreach (var contribution in taggedInContributions)
            {
                switch (contribution)
                {
                    case PostLightModel post:
                        TaggedInPosts.Add(post);
                        break;
                    case CommentLightModel comment:
                        TaggedInComments.Add(comment);
                        break;
                }
            }
        }

        public void Load(IMessage message)
        {
            TaggedInPosts.Clear();
            TaggedInComments.Clear();

            var taggedInContributions = facade.FindNewestNTagsOfUser(loggedUser, contributionsQuantity);
            foreach (var contribution in taggedInContributions)
            {
                switch (contribution)
                {
                    case PostLightModel post:
                        TaggedInPosts.Add(post);
                        break;
                    case CommentLightModel comment:
                        TaggedInComments.Add(comment);
                        break;
                }
            }
        }

        public ICommand TaggedInPostSelectedCommand { get; }

        private void TaggedInPostSelected(PostLightModel postLightModel) => mediator.Send(new TaggedInPostSelectedMessage { Post = postLightModel });

        public ICommand TaggedInCommentSelectedCommand { get; }

        private void TaggedInCommentSelected(CommentLightModel commentLightModel) => mediator.Send(new TaggedInCommentSelectedMessage { Comment = commentLightModel });

        private void ToggleVisibility(UserDetailToggleVisibilityMessage userDetailToggleVisibilityMessage)
        {
            visible = !visible;
        }
    }
}
