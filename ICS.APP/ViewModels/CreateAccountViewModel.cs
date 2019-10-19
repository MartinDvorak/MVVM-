using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Model.LightModel;
using TeamsManager.BL.Services;
using TeamsManager.DAL.Entities;

namespace TeamsManager.APP.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;
        public UserModel Model { get; set; } = new UserModel();
        public ICommand CreateAccountCommand { get; }
        public ICommand BackToLoginCommand { get; }
        public bool EmailWarning { get; set; } = false;
        public bool PasswordWarning { get; set; } = false;
        public bool RegisterPageVisibility { get; set; } = false;
        public bool ErrorWarning { get; set; } = false;

        public CreateAccountViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;
            CreateAccountCommand = new RelayCommand(createAccount, canCreateAccount);
            BackToLoginCommand = new RelayCommand(backToLogin);
            mediator.Register<CreateAccountMessage>(displayRegisterPage);
        }

        private void createAccount(object parameter)
        {
            hideWarnings();
            var hashedPasswords = getHashedPasswords(parameter).ToList();
            if (facade.AreUserDataValid(Model.FirstName, Model.LastName, Model.Email, hashedPasswords[0], hashedPasswords[1]))
            {
                this.Model.Password = hashedPasswords[0];
                var newUser = facade.Create(Model) as UserModel;
                UserLightModel newUserLightModel;
                try
                {
                    newUserLightModel = new UserLightModel
                    {
                        Id = newUser.Id,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Email = newUser.Email
                    };
                }
                catch (NullReferenceException)
                {
                    ErrorWarning = true;
                    return;
                }
                var message = new SuccessfullyRegisteredMessage() {User = newUserLightModel};
                RegisterPageVisibility = false;
                mediator.Send(message);
            }
            else
            {
                sendWarning(hashedPasswords[0], hashedPasswords[1]);
            }
        }

        private bool canCreateAccount(object parameter)
        {
            if (parameter == null)
                return false;

            var values = (object[])parameter;
            var password = (PasswordBox) values[0];
            var passwordConfirmation = (PasswordBox) values[1];
            return !string.IsNullOrEmpty(Model.FirstName)
                   && !string.IsNullOrEmpty(Model.LastName)
                   && !string.IsNullOrEmpty(Model.Email)
                   && !string.IsNullOrEmpty(password.Password)
                   && !string.IsNullOrEmpty(passwordConfirmation.Password);
        }

        private void backToLogin()
        {
            RegisterPageVisibility = false;
            var message = new BackToLoginMessage();
            mediator.Send(message);
        }
        
        private IEnumerable<string> getHashedPasswords(object parameter)
        {
            var separatedValues = new List<string>();
            var partialValues = (object[])parameter;
            if (partialValues[0] is PasswordBox password && partialValues[1] is PasswordBox passwordConfirmation)
            {
                separatedValues.Add(facade.HashPassword(password.Password));
                separatedValues.Add(facade.HashPassword(passwordConfirmation.Password));
            }
            return separatedValues;
        }

        private void sendWarning(string passwordHash, string passwordConfirmationHash)
        {
            if (!facade.IsEmailValid(Model.Email))
            {
                EmailWarning = true;
            }
            else if (!passwordHash.Equals(passwordConfirmationHash))
            {
                PasswordWarning = true;
            }
        }

        private void displayRegisterPage(CreateAccountMessage message) => RegisterPageVisibility = true;
        private void hideWarnings()
        {
            EmailWarning = false;
            PasswordWarning = false;
            ErrorWarning = false;
        }
    }
}
