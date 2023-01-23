using CaseApplication.WPF.Core;
using CaseApplication.WPF.Services;
using CaseApplication.WPF.View.StartUp;
using System.Windows.Controls;

namespace CaseApplication.WPF.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly PageService _pageService;
        private readonly ResizeMainWindowService _resizeMainWindowService;

        private Page? pageSource;
        private WindowMeasurements? windowMeasurements;

        public WindowMeasurements? WindowMeasurements
        {
            get => windowMeasurements;
            set
            {
                windowMeasurements = value;
                OnPropertyChanged(nameof(WindowMeasurements));
            }
        }

        public Page? PageSource { 
            get => pageSource; 
            set { 
                pageSource = value;
                OnPropertyChanged(nameof(PageSource));
            } 
        }

        public MainViewModel(
            PageService pageService,
            ResizeMainWindowService resizeMainWindowService)
        {
            _pageService = pageService;
            _resizeMainWindowService = resizeMainWindowService;

            _pageService.OnPageChanged += page => PageSource = page;
            _resizeMainWindowService.OnResizeMainWindow += size => WindowMeasurements = size;

            _resizeMainWindowService.ChangeSizeMainWindow(new WindowMeasurements(450, 450, 450, 450));
            _pageService.ChangePage(new AuthorizationPage());
        }
    }
}
