using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Settings = ViceCodeTestTask.Properties.Settings;

namespace ViceCodeTestTask.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
        private ApplicationViewModel _applicationViewModel;
        private string _password = "";
        private string _confirm = "";
        private IFirebaseAuthProvider AuthProvider => ApplicationViewModel.AuthProvider;
        private CancellationTokenSource cts;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password
        {
            get => _password;
            set
            {
                if (value != _password) _password = value;
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(PasswordsMatch));
            }
        }
        public string Confirm
        {
            get => _confirm;
            set
            {
                if (value != _confirm) _confirm = value;
                RaisePropertyChanged(nameof(Confirm));
                RaisePropertyChanged(nameof(PasswordsMatch));
            }
        }
        // Utilized by BooleanToVisibilityConverter to show matching error.
        public bool PasswordsMatch => !Password.Equals(Confirm);

        public SignUpViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand ToLogInCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToLogin();
        });

        public DelegateCommand SignUpCommand => new DelegateCommand(async() =>
        {
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(Email, Password, Username, false, cts.Token);
                _applicationViewModel.AuthLink = auth;

                if (auth != null) _applicationViewModel.ToMain();
            }
            catch (FirebaseAuthException ex)
            {
                var caption = "Failed to sign up.";
                var error = JsonConvert.DeserializeObject<FirebaseResponseRoot>(ex.ResponseData).Error;
                MessageBox.Show(
                    "Please make sure that you have entered your login and password correctly.\n" +
                    "\nError code: " + error.code + 
                    "\nError message: " + error.message, 
                    caption);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cts.Dispose();
            }
            
            //Settings.Default.Email = Email;
            //Settings.Default.Password = Password;
        });
    }
}
