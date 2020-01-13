using Prism.Commands;
using System;
using Firebase.Auth;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ViceCodeTestTask.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
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
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                _authLink = await AuthProvider.CreateUserWithEmailAndPasswordAsync(Email, SecureStringToString(Password2), 
                    Username, false, _cts.Token);

                if (_authLink != null) _applicationViewModel.ToMain();
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
                _cts.Dispose();
            }
        });
    }
}
