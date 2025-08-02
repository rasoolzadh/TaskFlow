// TaskFlow.MobileApp/ViewModels/BaseViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskFlow.MobileApp.ViewModels
{
    // --- FIX: Added the 'partial' keyword ---
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        public bool IsNotBusy => !IsBusy;
    }
}
