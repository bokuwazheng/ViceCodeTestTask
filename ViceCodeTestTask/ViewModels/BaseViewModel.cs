using System;
using Prism.Mvvm;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace ViceCodeTestTask.ViewModels
{
    public abstract class BaseViewModel : BindableBase 
    {
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
        public static IFirebaseAuthProvider AuthProvider => _serviceProvider.GetRequiredService<IFirebaseAuthProvider>();
        protected static FirebaseAuthLink _authLink;

        protected ApplicationViewModel _applicationViewModel;
        protected CancellationTokenSource _cts;
        private bool _isEnabled;
        private SecureString _password1;
        private SecureString _password2;

        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEnabled
        {
            get => _isEnabled; 
            set { SetProperty(ref _isEnabled, value); }
        }
        public SecureString Password1
        {
            get => _password1; 
            set { if (SetProperty(ref _password1, value)) ComparePasswords(); }
        }
        public SecureString Password2
        {
            get => _password2; 
            set { if (SetProperty(ref _password2, value)) ComparePasswords(); }
        }

        public void ComparePasswords()
        {
            if (_password1 == null || _password2 == null) return;
            IsEnabled = _password1.Length != 0 && _password2.Length != 0 &&
                SecureStringToString(_password1) == SecureStringToString(_password2);
        }

        public string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
