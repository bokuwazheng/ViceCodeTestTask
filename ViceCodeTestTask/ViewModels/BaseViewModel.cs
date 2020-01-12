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
using Prism.Mvvm;

namespace ViceCodeTestTask.ViewModels
{
    public abstract class BaseViewModel : Prism.Mvvm.BindableBase 
    {
        public BaseViewModel()
        {

        }
    }
}
