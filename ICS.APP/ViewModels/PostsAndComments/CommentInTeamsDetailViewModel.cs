using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.ContributionModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.ContributionLightModels;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels.PostsAndComments
{
    public class CommentInTeamsDetailViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;

        public bool Visibility { get; set; } = false;

        public CommentModel Model { get; set; }

        public ObservableCollection<UserLightModel> TaggedUsers { get; set; } = new ObservableCollection<UserLightModel>();

        private UserLightModel loggedUser { get; set; }

        public TeamModel LoadedTeam { get; set; } = null;

        public PostModel CorrespondingPostModel { get; set; }

        public CommentInTeamsDetailViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.facade = facade;
            this.messageBoxService = messageBoxService;
            this.mediator = mediator;

            mediator.Register<AddCommentTeamsMessage>(AddCommentHandle);
            mediator.Register<UserAuthenticatedMessage>(LoadLoggedUser);

            UserTagAddedCommand = new RelayCommand<UserLightModel>(AddUserTag);
            UserTagRemoveCommand = new RelayCommand<UserLightModel>(RemoveUserTag);

            AddNewCommentCommand = new RelayCommand(AddNewComment, CanAddNewComment);
            CloseCommentWindowCommand = new RelayCommand(CloseCommentWindow);
        }


        public void LoadLoggedUser(UserAuthenticatedMessage loggedInMessage)
        {
            loggedUser = loggedInMessage.User;
        }

        private void AddCommentHandle(AddCommentTeamsMessage addCommentTeamsMessage)
        {
            CorrespondingPostModel = addCommentTeamsMessage.PostModel;
            Model = new CommentModel();
            TaggedUsers = new ObservableCollection<UserLightModel>();
            Visibility = true;

            LoadedTeam = facade.GetDetail(CorrespondingPostModel.CorrespondingTeam) as TeamModel;
        }


        public ICommand UserTagAddedCommand { get; }

        private void AddUserTag(UserLightModel userLightModel)
        {
            if (!TaggedUsers.Contains(userLightModel))
            {
                TaggedUsers.Add(userLightModel);
            }
        }

        public ICommand UserTagRemoveCommand { get; }

        private void RemoveUserTag(UserLightModel userLightModel)
        {
            if (TaggedUsers.Contains(userLightModel))
            {
                TaggedUsers.Remove(userLightModel);
            }
        }


        public ICommand AddNewCommentCommand { get; set; }

        private Boolean CanAddNewComment() =>
            Model != null
            &&!string.IsNullOrWhiteSpace(Model.Content);

        private void AddNewComment()
        {
            Model.ParentContribution = new PostLightModel
            {
                Id = CorrespondingPostModel.Id
            };
            Model.Date = DateTime.Now;
            Model.Author = loggedUser;
            Model.AssociatedFiles = new List<ContributionFileLightModel>();
            Model.ContributionUserTags = new List<ContributionUserTagModel>();

            Model = facade.Create(Model) as CommentModel;
            facade.SetTaggedUserInPost(this.TaggedUsers, Model);
            Visibility = false;

            mediator.Send(new CommentAddedMessage());
        }


        public ICommand CloseCommentWindowCommand { get; }

        private void CloseCommentWindow()
        {
            Visibility = false;
            mediator.Send(new CloseCommentWindowTeamsMessage());
        }
    }
}
