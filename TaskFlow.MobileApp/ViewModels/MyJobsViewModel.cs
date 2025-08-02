// TaskFlow.MobileApp/ViewModels/MyJobsViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskFlow.MobileApp.Models;
using TaskFlow.MobileApp.Services;
using TaskFlow.MobileApp.Views;

namespace TaskFlow.MobileApp.ViewModels
{
    public partial class MyJobsViewModel : BaseViewModel
    {
        private readonly JobService _jobService;

        [ObservableProperty]
        private ObservableCollection<Job> _jobs;

        public MyJobsViewModel(JobService jobService)
        {
            Title = "My Assigned Jobs";
            _jobService = jobService;
            _jobs = new ObservableCollection<Job>();
        }

        [RelayCommand]
        private async Task GetJobsAsync()
        {
            // FIX: Use the generated public property 'IsBusy'
            if (IsBusy)
                return;

            try
            {
                // FIX: Use the generated public property 'IsBusy'
                IsBusy = true;
                var jobsList = await _jobService.GetMyJobsAsync();

                // FIX: Use the generated public property 'Jobs'
                if (Jobs == null)
                {
                    Jobs = new ObservableCollection<Job>();
                }

                Jobs.Clear();
                foreach (var job in jobsList)
                {
                    Jobs.Add(job);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to get jobs: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to retrieve job list.", "OK");
            }
            finally
            {
                // FIX: Use the generated public property 'IsBusy'
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Job job)
        {
            if (job == null)
                return;

            await Shell.Current.GoToAsync(nameof(JobDetailsPage), true, new Dictionary<string, object>
            {
                { "Job", job }
            });
        }
    }
}
