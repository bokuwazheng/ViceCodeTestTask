using Firebase.Auth;
using Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Settings = ViceCodeTestTask.Properties.Settings;

namespace ViceCodeTestTask.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        //#region Singleton implementation
        //private static readonly ApplicationViewModel _instance = new ApplicationViewModel();
        //public static ApplicationViewModel Current => _instance;

        //static ApplicationViewModel() { }
        //#endregion

        private FirebaseAuth _firebaseAuth;

        private static readonly IServiceProvider _serviceProvider = _serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile("config.json").Build())
            .AddHttpClient()
            .AddTransient<IFirebaseAuthProvider, FirebaseAuthProvider>(x =>
            {
                var config = x.GetRequiredService<IConfiguration>();
                var apiKey = config.GetValue<string>("apiKey");
                return new FirebaseAuthProvider(apiKey, x.GetRequiredService<IHttpClientFactory>());
            })
            .BuildServiceProvider();

        private static readonly IConfiguration _config = _serviceProvider.GetRequiredService<IConfiguration>();

        public static IFirebaseAuthProvider AuthProvider => _serviceProvider.GetRequiredService<IFirebaseAuthProvider>();
        public FirebaseAuthLink AuthLink { get; set; }

        // нелья приватный, выдаёt замл ошибку, надо перенести в свойства, наверное
        public ApplicationViewModel()
        {
            _viewModels = new List<BaseViewModel>()
            {
                new LoginViewModel(this),
                new MainViewModel(this),
                new UserProfileViewModel(this),
                new SignUpViewModel(this),
                new UserSettingsViewModel(this),
                new RecoveryViewModel(this)
            };

            _firebaseAuth = new FirebaseAuth();

            if (_firebaseAuth.User == null)
            {
                ToSignUp();
            }
            else
            {
                ToMain();
            }
        }

        private List<BaseViewModel> _viewModels;

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value) _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel));
            }
        }

        public string FirebaseApiKey { get; }

        public void ToLogin()
        {
            CurrentViewModel = _viewModels[0];
        }

        public void ToMain()
        {
            CurrentViewModel = _viewModels[1];
        }

        public void ToSettings()
        {
            CurrentViewModel = _viewModels[2];
        }

        public void ToSignUp()
        {
            CurrentViewModel = _viewModels[3];
        }

        public void ToUserSettings()
        {
            CurrentViewModel = _viewModels[4];
        }

        public void ToRecovery()
        {
            CurrentViewModel = _viewModels[5];
        }
    }
}
