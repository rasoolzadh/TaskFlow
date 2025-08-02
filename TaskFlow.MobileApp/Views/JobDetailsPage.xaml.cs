// TaskFlow.MobileApp/Views/JobDetailsPage.xaml.cs

using TaskFlow.MobileApp.ViewModels;

namespace TaskFlow.MobileApp.Views;

public partial class JobDetailsPage : ContentPage
{
    public JobDetailsPage(JobDetailsViewModel viewModel)
    {
        InitializeComponent();

        // Set the BindingContext of the page to the provided ViewModel.
        // This is how the XAML elements know which properties and commands to bind to.
        BindingContext = viewModel;
    }
}
