using System.Collections.Generic;

namespace ViceCodeTestTask.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private List<BaseViewModel> _viewModels;

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set { SetProperty(ref _currentViewModel, value); }
        }

        public ApplicationViewModel()
        {
            _viewModels = new List<BaseViewModel>()
            {
                new SignUpViewModel(this),
                new LoginViewModel(this),
                new MainViewModel(this),
                new UserSettingsViewModel(this),
                new RecoveryViewModel(this)
            };

            ToLogin();
        }

        public void ToSignUp()
        {
            CurrentViewModel = _viewModels[0];
        }

        public void ToLogin()
        {
            CurrentViewModel = _viewModels[1];
        }

        public void ToMain()
        {
            CurrentViewModel = _viewModels[2];
        }

        public void ToUserSettings()
        {
            CurrentViewModel = _viewModels[3];
        }

        public void ToRecovery()
        {
            CurrentViewModel = _viewModels[4];
        }
    }
}
