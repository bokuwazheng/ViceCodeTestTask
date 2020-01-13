using Prism.Commands;
using System;
using Firebase.Auth;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Security;

namespace ViceCodeTestTask.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        private SecureString _deletionPassword;

        public SecureString CurrentPassword { get; set; }
        public SecureString DeletionPassword
        {
            get => _deletionPassword;
            set { SetProperty(ref _deletionPassword, value); RaisePropertyChanged(nameof(DeletionEnabled)); }
        }
        public bool DeletionEnabled => DeletionPassword?.Length >= 6;

        public UserSettingsViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand ToMainCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToMain();
        });

        public DelegateCommand ToRecoveryCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToRecovery();
        });

        public DelegateCommand ChangePasswodCommand => new DelegateCommand(async() =>
        {
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                // Confirm password.
                await AuthProvider.SignInWithEmailAndPasswordAsync(_authLink.User.Email,
                    SecureStringToString(CurrentPassword),
                    _cts.Token).ConfigureAwait(false);
                // Change password.
                await AuthProvider.ChangeUserPasswordAsync(_authLink.FirebaseToken, 
                    SecureStringToString(Password2), 
                    _cts.Token).ConfigureAwait(false);
                // Notify user.
                MessageBox.Show("Success! Your password has been changed!");
            }
            catch (FirebaseAuthException ex)
            {
                var caption = "Failed to change password.";
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
                _cts.Dispose();
            }
        });

        public DelegateCommand DeleteAccountCommand => new DelegateCommand(async() =>
        {
            var message = "Are you really, really sure you want to delete your ViceCodeTestTask user account? " +
            "It would be a shame to see you go!\n" +
            "\nTo delete your account, please click OK button.";

            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                // Confirm password.
                await AuthProvider.SignInWithEmailAndPasswordAsync(_authLink.User.Email,
                    SecureStringToString(DeletionPassword),
                    _cts.Token).ConfigureAwait(false);
                // Reassure.
                var reassurance = MessageBox.Show(message, "Are you sure?", MessageBoxButtons.OKCancel);
                if (reassurance == DialogResult.Cancel) return;
                // Delete.
                await AuthProvider.DeleteUserAsync(_authLink.FirebaseToken, _cts.Token).ConfigureAwait(false);
                // Log out.
                _applicationViewModel.ToLogin();
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
                _cts.Dispose();
            }
        });
    }
}
