using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.APP.Services;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.FileModels;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Model.LightModel.FileLightModels;
using TeamsManager.BL.Services;
using TeamsManager.DAL.Entities.Files;

namespace TeamsManager.APP.ViewModels
{
    public class UserDetailViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMessageBoxService messageBoxService;
        private readonly IMediator mediator;

        public bool Visibility { get; set; } = false;
        public string ImageName { get; set; }
        public UserModel Model { get; set; } = new UserModel();

        public UserDetailViewModel(IBusinessLogicFacade facade, IMessageBoxService messageBoxService, IMediator mediator)
        {
            this.mediator = mediator;
            this.facade = facade;
            this.messageBoxService = messageBoxService;

            mediator.Register<UserDetailToggleVisibilityMessage>(UserDetailToggleVisibility);

            SaveAccountChangesCommand = new RelayCommand(SaveAccountChanges, CanSaveAccountChanges);
            ChangePasswordCommand = new RelayCommand(ChangePassword,CanChangePassword);
            AddImageCommand = new RelayCommand(AddImage);
        }

        private bool CanChangePassword(object passwordBoxes)
        {
            if (passwordBoxes == null)
                return false;

            var values = (object[])passwordBoxes;
            var password = (PasswordBox)values[0];
            var passwordConfirmation = (PasswordBox)values[1];
            return !string.IsNullOrEmpty(password.Password)
                   && !string.IsNullOrEmpty(passwordConfirmation.Password)
                   && password.Password == passwordConfirmation.Password;
        }
    

        private void ChangePassword(object passwordBoxes)
        {
            var values = (object[])passwordBoxes;
            var hash = getHashedPassword((PasswordBox) values[0]);
            Model.Password = hash;
            ((PasswordBox)values[0]).Clear();
            ((PasswordBox)values[1]).Clear();
            SaveAccountChanges();
        }

        private string getHashedPassword(PasswordBox passwordBox)
        {
            return facade.HashPassword(passwordBox.Password);
        }
        private ProfileImageModel newProfilePicture { get; set; }

        public ICommand AddImageCommand { get; }
        public ICommand ChangePasswordCommand { get; set; }
        public void Load(UserLightModel userLightModel)
        {
            Model = facade.GetDetail(userLightModel) as UserModel;
            this.ImageName = "";
            this.newProfilePicture = null;
        }

        public void Save()
        {
            if (Model.Id == null)
            {
                Model = facade.Create(Model) as UserModel;
            }
            else
            {
                facade.Update(Model);
            }
        }

        public void Delete()
        {
            if (Model.Id != null)
            {
                try
                {
                    facade.Delete(Model);
                }
                catch
                {
                    messageBoxService.Show($"Deleting of {Model?.FirstName} {Model?.LastName} failed!", "Deleting failed", MessageBoxButton.OK);
                    throw;
                }

                Model = null;
            }
        }

        private void UserDetailToggleVisibility(UserDetailToggleVisibilityMessage userDetailToggleVisibilityMessage)
        {
            Visibility = !Visibility;
            if (Visibility)
            {
                Model = facade.GetDetail(userDetailToggleVisibilityMessage.UserLightModel) as UserModel;
                this.ImageName = "";
                this.newProfilePicture = null;
            }
        }

        public ICommand SaveAccountChangesCommand { get; set; }

        private Boolean CanSaveAccountChanges() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Email)
            && !string.IsNullOrWhiteSpace(Model.FirstName)
            && !string.IsNullOrWhiteSpace(Model.LastName)
            && !string.IsNullOrWhiteSpace(Model.UserDescription);

        public void SaveAccountChanges()
        {
            if (!String.IsNullOrWhiteSpace(this.ImageName))
            {
                facade.Update(this.newProfilePicture);
                Model.ProfilePicture = new ProfileImageLightModel{
                    Id = this.newProfilePicture.Id,
                    Content = this.newProfilePicture.Content,
                    PictureFormat = this.newProfilePicture.PictureFormat
                };
            }
            facade.Update(Model);

            UserLightModel lightModel = new UserLightModel
            {
                Email = Model.Email,
                FirstName = Model.FirstName,
                Id = Model.Id,
                LastName = Model.LastName,
                UserDescription = Model.UserDescription,
                ProfilePicture = Model.ProfilePicture
            };

            mediator.Send(new AccountUpdatedMessage{UserLightModel = lightModel});
        }
        private void AddImage(object obj)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Document",
                DefaultExt = ".png",
                Filter =
                    "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.ImageName = openFileDialog.SafeFileName;
                var avatar = System.Drawing.Image.FromFile(openFileDialog.FileName);
                this.newProfilePicture = new ProfileImageModel()
                {
                    Content = facade.ConvertImageToByteArray(avatar),
                    PictureFormat = facade.ConvertFileExtenstionToEnum(openFileDialog.DefaultExt),
                    FileName = openFileDialog.SafeFileName,
                    Id = Model.ProfilePicture.Id
                };
            }
        }
    }
}
