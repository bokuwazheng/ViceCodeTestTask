using Prism.Commands;
using System;
using Firebase.Auth;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ViceCodeTestTask.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand ToSignUpCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToSignUp();
        });

        public DelegateCommand ToRecoveryCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToRecovery();
        });

        public DelegateCommand LogInCommand => new DelegateCommand(async() =>
        {
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                _authLink = await AuthProvider.SignInWithEmailAndPasswordAsync(Email, SecureStringToString(Password1)).
                ConfigureAwait(false);

                if (_authLink != null) _applicationViewModel.ToMain();
            }
            catch (FirebaseAuthException ex)
            {
                var caption = "Failed to log in.";
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
