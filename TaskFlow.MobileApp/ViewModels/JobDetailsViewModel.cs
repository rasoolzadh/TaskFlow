// TaskFlow.MobileApp/ViewModels/JobDetailsViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TaskFlow.MobileApp.Models;
using TaskFlow.MobileApp.Services;

namespace TaskFlow.MobileApp.ViewModels
{
    [QueryProperty(nameof(Job), "Job")]
    public class JobDetailsViewModel : BaseViewModel
    {
        private readonly JobService _jobService;
        private Job _job;
        private ImageSource? _completedWorkPhoto;

        public Job Job
        {
            get => _job;
            set => SetProperty(ref _job, value);
        }

        // --- MANUAL IMPLEMENTATION of the Photo Property ---
        public ImageSource? CompletedWorkPhoto
        {
            get => _completedWorkPhoto;
            set => SetProperty(ref _completedWorkPhoto, value);
        }

        public ICommand UpdateStatusCommand { get; }
        public ICommand GetDirectionsCommand { get; }
        public ICommand AddPhotoCommand { get; }

        public JobDetailsViewModel(JobService jobService)
        {
            Title = "Job Details";
            _jobService = jobService;
            _job = new Job();

            UpdateStatusCommand = new AsyncRelayCommand<JobStatus>(UpdateStatusCommandAsync);
            GetDirectionsCommand = new AsyncRelayCommand(GetDirectionsAsync);
            AddPhotoCommand = new AsyncRelayCommand(AddPhotoAsync);
        }

        private async Task UpdateStatusCommandAsync(JobStatus newStatus)
        {
            if (IsBusy || Job is null) return;
            try
            {
                IsBusy = true;
                bool success = await _jobService.UpdateJobStatusAsync(Job.Id, newStatus);
                if (success)
                {
                    Job.Status = newStatus;
                    OnPropertyChanged(nameof(Job));
                    await Shell.Current.DisplayAlert("Success", "Job status has been updated.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update job status.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating status: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "An unexpected error occurred.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetDirectionsAsync()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(Job?.Client?.Address))
            {
                await Shell.Current.DisplayAlert("No Address", "This client does not have an address specified.", "OK");
                return;
            }
            try
            {
                var address = Uri.EscapeDataString(Job.Client.Address);
                var uri = new Uri($"https://www.google.com/maps/search/?api=1&query={address}");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to open browser: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not open web browser for directions.", "OK");
            }
        }

        private async Task AddPhotoAsync()
        {
            if (IsBusy) return;

            try
            {
                FileResult? photo;
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    photo = await MediaPicker.Default.CapturePhotoAsync();
                }
                else
                {
                    photo = await MediaPicker.Default.PickPhotoAsync();
                }

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    CompletedWorkPhoto = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding photo: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Unable to add photo.", "OK");
            }
        }
    }
}
