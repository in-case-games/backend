using CaseApplication.WPF.Services;

namespace CaseApplication.WPF.ViewModel.StartUp
{
    public class AuthorizationViewModel
    {
        private readonly PageService _pageService;
        private readonly ResizeMainWindowService _resizeMainWindowService;

        public AuthorizationViewModel(
            PageService pageService, 
            ResizeMainWindowService resizeMainWindowService)
        {
            _pageService = pageService;
            _resizeMainWindowService = resizeMainWindowService;
        }
    }
}
