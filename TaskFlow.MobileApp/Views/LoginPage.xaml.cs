// TaskFlow.MobileApp/Views/LoginPage.xaml.cs

using TaskFlow.MobileApp.ViewModels;

namespace TaskFlow.MobileApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        // Set the BindingContext of the page to the provided ViewModel.
        // This is how the XAML elements (Entry, Button, etc.) know which
        // properties and commands to bind to.
        BindingContext = viewModel;
    }
}
