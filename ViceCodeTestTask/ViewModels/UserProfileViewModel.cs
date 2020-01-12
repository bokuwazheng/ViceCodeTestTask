using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViceCodeTestTask.ViewModels
{
    class UserProfileViewModel : BaseViewModel
    {
        private ApplicationViewModel _applicationViewModel;

        public UserProfileViewModel(ApplicationViewModel applicationViewModel)
        {
            _applicationViewModel = applicationViewModel;
        }
    }
}
