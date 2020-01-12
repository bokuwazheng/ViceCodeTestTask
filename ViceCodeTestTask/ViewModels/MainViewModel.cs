using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViceCodeTestTask.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private ApplicationViewModel _applicationViewModel;
        public string Username => _applicationViewModel.AuthLink?.User.DisplayName;
        public string Email => _applicationViewModel.AuthLink?.User.Email;
        public string SignedUpSince => _applicationViewModel.AuthLink?.Created.Date.ToShortDateString();

        public MainViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
            _applicationViewModel.PropertyChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(Username));
                RaisePropertyChanged(nameof(Email));
                RaisePropertyChanged(nameof(SignedUpSince));
            };
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
