using Prism.Commands;
using System;
using Firebase.Auth;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ViceCodeTestTask.ViewModels
{
    public class RecoveryViewModel : BaseViewModel
    {
        public RecoveryViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand<string> SendInstructionsCommand => new DelegateCommand<string>(async (email) =>
        {
            _cts = new CancellationTokenSource();
            _cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                await AuthProvider.SendPasswordResetEmailAsync(email, _cts.Token).ConfigureAwait(false);
                MessageBox.Show("Please check you email.");
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

        public DelegateCommand ToLogInCommand => new DelegateCommand(() =>
        {
            _applicationViewModel.ToLogin();
        });
    }
}
