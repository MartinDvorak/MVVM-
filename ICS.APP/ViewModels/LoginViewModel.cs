using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Facade;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Model;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class LoginViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IBusinessLogicFacade facade;
        private readonly IMediator mediator;
        private string email;
        public bool LoginPageVisibility { get; set; } = true;//false; //TODO flip value to enable authentication process
        public string Email
        {
            get => this.email;
            set
            {
                if (!string.Equals(this.email, value))
                {
                    this.email = value;
                }
            }
        }

        public ICommand AuthenticateCommand { get; }
        public ICommand CreateAccountCommand { get; }

        public LoginViewModel(IBusinessLogicFacade facade, IMediator mediator)
        {
            this.facade = facade;
            this.mediator = mediator;

            AuthenticateCommand = new RelayCommand(authenticate, canAuthenticate);
            CreateAccountCommand = new RelayCommand(displayRegisterPage);
            mediator.Register<BackToLoginMessage>(displayLoginPage);
        }

        private void authenticate(object parameter)
        {
            if (!(parameter is PasswordBox passwordBox)) return;
            var passwordHash = facade.HashPassword(passwordBox.Password);
            passwordBox.Clear();

            var user = facade.FindUserByEmail(Email);
            if (user != null)
            {
                var userDetail = facade.GetDetail(user) as UserModel;
                if (passwordHash.Equals(userDetail.Password))
                {
                    var message = new UserAuthenticatedMessage {User = user};
                    mediator.Send(message);
                    LoginPageVisibility = !LoginPageVisibility;
                }
            }
        }

        private bool canAuthenticate(object parameter)
        {
            if (!(parameter is PasswordBox passwordBox)) return false;
            var pwd = (string) passwordBox.Password;
            return !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(Email);

        }

        private void displayLoginPage(IMessage backToLoginMessage)
        {
            LoginPageVisibility = true;
        }

        private void displayRegisterPage()
        {
            LoginPageVisibility = false;
            mediator.Send(new CreateAccountMessage());
        }
    }
}
