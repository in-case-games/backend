using CaseApplication.WPF.Services;

namespace CaseApplication.WPF.ViewModel.StartUp
{
    public class AuthorizationViewModel
    {
        private readonly PageService _pageService;

        public AuthorizationViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
    }
}
