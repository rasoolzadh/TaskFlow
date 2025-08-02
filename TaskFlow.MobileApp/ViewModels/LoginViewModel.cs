// TaskFlow.MobileApp/ViewModels/LoginViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskFlow.MobileApp.ViewModels
{
    // --- FIX: Added the 'partial' keyword ---
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public LoginViewModel()
        {
            Title = "User Login";
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter both email and password.", "OK");
                    return;
                }

                await Task.Delay(2000);

                await Shell.Current.DisplayAlert("Success", "Login would be successful!", "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login failed: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "An unexpected error occurred during login.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
