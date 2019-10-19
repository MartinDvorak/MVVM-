using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class PostDetailViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;


        public bool Visibility { get; set; } = false;

        private UserLightModel loggedUser { get; set; }

        public PostModel Model { get; set; }
        public TeamModel LoadedTeam { get; set; } = null;

        public ObservableCollection<UserLightModel> TaggedUsers { get; set; }

        public PostDetailViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.facade = facade;
            this.messageBoxService = messageBoxService;
            this.mediator = mediator;

            mediator.Register<AddPostMessage>(Load);
            mediator.Register<UserAuthenticatedMessage>(LoadLoggeduser);

            mediator.Register<MyTeamSelectedMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamInformationToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<EditTeamMembersToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<ShowTeamPostsToggleVisibilityMessage>(TurnVisibilityToFalse);
            mediator.Register<CreateNewTeamMessage>(TurnVisibilityToFalse);

            UserTagAddedCommand = new RelayCommand<UserLightModel>(UserTagAdded);
            UserTagRemoveCommand = new RelayCommand<UserLightModel>(UserTagRemove);

            AddFileCommand = new RelayCommand(AddFile);
            mediator.Register<AddFileMessage>(AddFileHandle);

            AddNewPostCommand = new RelayCommand(AddNewPost, CanAddNewPost);
        }


        private void TurnVisibilityToFalse(IMessage obj)
        {
            this.Visibility = false;
            Model = null;
            LoadedTeam = null;
            TaggedUsers = null;
        }


        public void Load(AddPostMessage addPostMessage)
        {
            LoadedTeam = addPostMessage.TeamModel;
            Visibility = true;
            Model = new PostModel();
            TaggedUsers = new ObservableCollection<UserLightModel>();
        }

        public void LoadLoggeduser(UserAuthenticatedMessage loggedInMessage)
        {
            loggedUser = loggedInMessage.User;
        }

        public ICommand AddNewPostCommand { get; set; }

        private Boolean CanAddNewPost() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Title)
            && !string.IsNullOrWhiteSpace(Model.Content);

        private void AddNewPost()
        {
            Model.Author = loggedUser;
            Model.Comments = new List<CommentModel>();
            Model.CorrespondingTeam = facade.ConvertTeamModelToTeamLightModel(LoadedTeam);
            Model.Date = DateTime.Now;
            Model.AssociatedFiles = new List<ContributionFileLightModel>();
            Model.ContributionUserTags = new List<ContributionUserTagModel>();

            Model = facade.Create(Model) as PostModel;
            facade.SetTaggedUserInPost(this.TaggedUsers,Model);

            mediator.Send(new ShowTeamPostsToggleVisibilityMessage { TeamModel = LoadedTeam });
            mediator.Send(new PostAddedMessage());
        }       

        public ICommand UserTagAddedCommand { get; }
        public ICommand UserTagRemoveCommand { get; }

        private void UserTagAdded(UserLightModel userLightModel)
        {
            if (!TaggedUsers.Contains(userLightModel))
            {
                TaggedUsers.Add(userLightModel);
            }
        }

        private void UserTagRemove(UserLightModel userLightModel)
        {
            if (TaggedUsers.Contains(userLightModel))
            {
                TaggedUsers.Remove(userLightModel);
            }
        }

        public ICommand AddFileCommand { get; }

        private void AddFile() => mediator.Send(new AddFileMessage());

        public void AddFileHandle(AddFileMessage addFileMessage)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".jpeg"; // Default file extension
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"; // Filter files by extension

            //display
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fn = dlg.FileName;
            }
        }
    }
}
