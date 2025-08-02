// TaskFlow.MobileApp/MauiProgram.cs

using Microsoft.Extensions.Logging;
using TaskFlow.MobileApp.Services;
using TaskFlow.MobileApp.ViewModels;
using TaskFlow.MobileApp.Views;

namespace TaskFlow.MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- Service and ViewModel Registration ---

        // Register Services as Singleton
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<JobService>();

        // Register ViewModels as Transient
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MyJobsViewModel>();
        builder.Services.AddTransient<JobDetailsViewModel>(); // <-- Add this line

        // Register Views (Pages) as Transient
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MyJobsPage>();
        builder.Services.AddTransient<JobDetailsPage>(); // <-- Add this line


        return builder.Build();
    }
}
