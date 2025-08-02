// TaskFlow.MobileApp/ViewModels/JobDetailsViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input; // Required for ICommand
using TaskFlow.MobileApp.Models;
using TaskFlow.MobileApp.Services;

namespace TaskFlow.MobileApp.ViewModels
{
    [QueryProperty(nameof(Job), "Job")]
    // NOTE: This class is no longer partial as we are writing the code manually.
    public class JobDetailsViewModel : BaseViewModel
    {
        private readonly JobService _jobService;
        private Job _job;

        // --- MANUAL IMPLEMENTATION of the 'Job' property ---
        public Job Job
        {
            get => _job;
            set => SetProperty(ref _job, value);
        }

        // --- MANUAL IMPLEMENTATION of the 'UpdateStatusCommand' ---
        public ICommand UpdateStatusCommand { get; }

        public JobDetailsViewModel(JobService jobService)
        {
            Title = "Job Details";
            _jobService = jobService;
            _job = new Job();

            // We create the command manually in the constructor.
            UpdateStatusCommand = new AsyncRelayCommand<JobStatus>(UpdateStatusCommandAsync);
        }

        private async Task UpdateStatusCommandAsync(JobStatus newStatus)
        {
            if (IsBusy || Job is null)
                return;

            try
            {
                IsBusy = true;
                bool success = await _jobService.UpdateJobStatusAsync(Job.Id, newStatus);

                if (success)
                {
                    // Update the property, which will notify the UI
                    Job.Status = newStatus;
                    OnPropertyChanged(nameof(Job)); // Manually notify the UI that the Job object has changed

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
    }
}
