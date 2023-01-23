using CaseApplication.WPF.Service;

namespace CaseApplication.WPF.ViewModel.StartUp
{
    public class AuthorizationPageViewModel
    {
        private readonly PageService _pageService;

        public AuthorizationPageViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
    }
}
