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

        public MainViewModel MainViewModel => _provider!.GetRequiredService<MainViewModel>();
        public AuthorizationViewModel AuthViewModel => _provider!.GetRequiredService<AuthorizationViewModel>();
        public SignInViewModel SignInViewModel => _provider!.GetRequiredService<SignInViewModel>();
        public SignUpViewModel SignUpViewModel => _provider!.GetRequiredService<SignUpViewModel>();


        public static void Init()
        {
            var services = new ServiceCollection();
            
            services.AddTransient<MainViewModel>();
            services.AddTransient<AuthorizationViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();

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
