using Prism.Commands;

namespace ViceCodeTestTask.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public new string Username => _authLink?.User.DisplayName;
        public new string Email => _authLink?.User.Email;
        public string SignedUpSince => _authLink?.Created.Date.ToShortDateString();
        public string Verified => (bool)_authLink?.User.IsEmailVerified ? "Verified" : "Not verified";

        public MainViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand ToUserSettingsCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToUserSettings();
        });

        public DelegateCommand ToLogInCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToLogin();
        });
    }
}
