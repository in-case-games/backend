using CaseApplication.WebClient.Services;
using CaseApplication.WPF.Service;
using CaseApplication.WPF.ViewModel;
using CaseApplication.WPF.ViewModel.StartUp;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace CaseApplication.WPF
{
    public class ViewModelLocator
    {
        private static ServiceProvider? _provider;

        public MainWindowViewModel MainViewModel => _provider!.GetRequiredService<MainWindowViewModel>();
        public AuthorizationPageViewModel AuthViewModel => _provider!.GetRequiredService<AuthorizationPageViewModel>();
        
        public static void Init()
        {
            var services = new ServiceCollection();
            
            services.AddTransient<MainWindowViewModel>();

            services.AddScoped<AuthorizationPageViewModel>();

            services.AddSingleton<PageService>();
            services.AddSingleton<ResponseHelper>();

            _provider = services.BuildServiceProvider();

            foreach(var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }
    }
}
