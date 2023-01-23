using CaseApplication.WPF.Core;
using CaseApplication.WPF.Service;
using CaseApplication.WPF.View.StartUp;
using System.Windows.Controls;

namespace CaseApplication.WPF.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly PageService _pageService;

        private Page? pageSource;

        public Page? PageSource { 
            get => pageSource; 
            set { 
                pageSource = value;
                OnPropertyChanged(nameof(PageSource));
            } 
        }

        public MainWindowViewModel(PageService pageService)
        {
            _pageService = pageService;

            _pageService.OnPageChanged += page => PageSource = page;
            _pageService.ChangePage(new AuthorizationPage());
        }
    }
}
