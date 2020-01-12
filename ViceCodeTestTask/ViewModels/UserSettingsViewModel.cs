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
    public class UserSettingsViewModel : BaseViewModel
    {
        private ApplicationViewModel _applicationViewModel;
        private string _newPassword = "";
        private string _newConfirm = "";

        private IFirebaseAuthProvider AuthProvider => ApplicationViewModel.AuthProvider;
        private CancellationTokenSource cts;

        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (value != _newPassword) _newPassword = value;
                RaisePropertyChanged(nameof(NewPassword));
                RaisePropertyChanged(nameof(PasswordsMatch));
            }
        }
        public string NewConfirm
        {
            get => _newConfirm;
            set
            {
                if (value != _newConfirm) _newConfirm = value;
                RaisePropertyChanged(nameof(NewConfirm));
                RaisePropertyChanged(nameof(PasswordsMatch));
            }
        }
        public string DeletionPassword { get; set; }
        // Utilized by BooleanToVisibilityConverter to show matching error.
        public bool PasswordsMatch => !NewPassword.Equals(NewConfirm);

        public UserSettingsViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand ChangePasswodCommand => new DelegateCommand(async() =>
        {
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(180.0));
            var token = _applicationViewModel.AuthLink.FirebaseToken;

            try
            {
                await AuthProvider.ChangeUserPasswordAsync(token, NewPassword, cts.Token);
            }
            catch (FirebaseAuthException ex)
            {
                var caption = "Failed to change password.";
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

        public DelegateCommand ToMainCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToMain();
        });

        public DelegateCommand DeleteAccountCommand => new DelegateCommand(async() =>
        {
            var message = "Are you really, really sure you want to delete your ViceCodeTestTask user account? " +
            "It would be a shame to see you go!\n" +
            "\nTo delete your account, please click OK button.";

            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(180.0));
            var authLink = _applicationViewModel.AuthLink;

            try
            {
                // Confirm password.
                await AuthProvider.SignInWithEmailAndPasswordAsync(authLink.User.Email,
                    DeletionPassword,
                    cts.Token).ConfigureAwait(false);
                // Reassure.
                var reassurance = MessageBox.Show(message, "Are you sure?", MessageBoxButtons.OKCancel);
                if (reassurance == DialogResult.Cancel) return;
                // Delete.
                await AuthProvider.DeleteUserAsync(authLink.FirebaseToken, cts.Token).ConfigureAwait(false);
            }
            catch (FirebaseAuthException ex)
            {
                var caption = "Something went wrong!";
                var error = JsonConvert.DeserializeObject<FirebaseResponseRoot>(ex.ResponseData).Error;
                MessageBox.Show(
                    "Please make sure that you have entered your password correctly.\n" +
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


    }
}
