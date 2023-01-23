using CaseApplication.WPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.WPF.ViewModel.StartUp
{
    public class SignInViewModel
    {
        private readonly PageService _pageService;

        public SignInViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
    }
}
