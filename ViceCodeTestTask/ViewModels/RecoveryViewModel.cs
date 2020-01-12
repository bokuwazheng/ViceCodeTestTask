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
using Prism.Mvvm;

namespace ViceCodeTestTask.ViewModels
{
    public class RecoveryViewModel : BaseViewModel
    {
        private ApplicationViewModel _applicationViewModel;

        private IFirebaseAuthProvider AuthProvider => ApplicationViewModel.AuthProvider;
        private CancellationTokenSource cts;

        public RecoveryViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }

        public DelegateCommand<string> SendInstructionsCommand => new DelegateCommand<string>(async (email) =>
        {
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(180.0));

            try
            {
                await AuthProvider.SendPasswordResetEmailAsync(email, cts.Token).ConfigureAwait(false);
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
                cts.Dispose();
            }
        });
    }
}
