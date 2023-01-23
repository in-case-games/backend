using CaseApplication.WPF.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.WPF.ViewModel.StartUp
{
    public class SignUpViewModel
    {
        private readonly PageService _pageService;

        public SignUpViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
    }
}
