using System.Windows.Input;
using TeamsManager.APP.Commands;
using TeamsManager.BL.Messages;
using TeamsManager.BL.Services;

namespace TeamsManager.APP.ViewModels
{
    public class MainViewModel : BaseViewModel.BaseViewModel
    {
        private readonly IMediator mediator;
        public bool LoginPageVisibility { get; set; } = true;//false; //TODO flip value to enable authentication process
        public bool RegisterPageVisibility { get; set; } = false;

        public MainViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            mediator.Register<UserAuthenticatedMessage>(toggleLoginPage);
            mediator.Register<CreateAccountMessage>(displayRegisterPage);
            mediator.Register<SuccessfullyRegisteredMessage>(toggleLoginPage);
        }

        private void toggleLoginPage(IMessage message) => LoginPageVisibility = !LoginPageVisibility;
        private void displayRegisterPage(CreateAccountMessage message) => RegisterPageVisibility = true;
    }
}
