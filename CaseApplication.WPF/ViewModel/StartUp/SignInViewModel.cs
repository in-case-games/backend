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
        private readonly ResizeMainWindowService _resizeMainWindowService;

        public SignInViewModel(
            PageService pageService, 
            ResizeMainWindowService resizeMainWindowService)
        {
            _pageService = pageService;
            _resizeMainWindowService = resizeMainWindowService;
        }
    }
}
