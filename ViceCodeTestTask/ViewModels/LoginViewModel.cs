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
    public class LoginViewModel : BaseViewModel
    {
        private IFirebaseAuthProvider AuthProvider => ApplicationViewModel.AuthProvider;
        private CancellationTokenSource cts;
        private ApplicationViewModel _applicationViewModel;
        public string Email { get; set; }
        public string Password { get; set; }


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
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                var auth = await AuthProvider.SignInWithEmailAndPasswordAsync(Email, Password).ConfigureAwait(false);
                _applicationViewModel.AuthLink = auth;

                if (auth != null) _applicationViewModel.ToMain();
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
                cts.Dispose();
            }
        });

        public DelegateCommand AuthorizeCommand => new DelegateCommand(() =>
        {

        });



    }
}
