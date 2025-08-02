// TaskFlow.MobileApp/AppShell.xaml.cs

using TaskFlow.MobileApp.Views;

namespace TaskFlow.MobileApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // --- Register Routes for Navigation ---
        Routing.RegisterRoute(nameof(MyJobsPage), typeof(MyJobsPage));
        Routing.RegisterRoute(nameof(JobDetailsPage), typeof(JobDetailsPage)); // <-- Add this line
    }
}
