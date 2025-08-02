// TaskFlow.MobileApp/ViewModels/LoginViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskFlow.MobileApp.Models;
using TaskFlow.MobileApp.Services;
using TaskFlow.MobileApp.Views;

namespace TaskFlow.MobileApp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public LoginViewModel(AuthService authService)
        {
            Title = "User Login";
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            // FIX: Use the generated public property 'IsBusy'
            if (IsBusy)
                return;

            try
            {
                // FIX: Use the generated public property 'IsBusy'
                IsBusy = true;

                // FIX: Use the generated public properties 'Email' and 'Password'
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter both email and password.", "OK");
                    return;
                }

                var loginRequest = new LoginRequest { Email = Email, Password = Password };
                var token = await _authService.LoginAsync(loginRequest);

                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.Default.SetAsync("auth_token", token);
                    await Shell.Current.GoToAsync(nameof(MyJobsPage));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login failed: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
            finally
            {
                // FIX: Use the generated public property 'IsBusy'
                IsBusy = false;
            }
        }
    }
}
