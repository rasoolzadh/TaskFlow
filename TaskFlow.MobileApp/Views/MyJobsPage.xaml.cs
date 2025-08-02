// TaskFlow.MobileApp/Views/MyJobsPage.xaml.cs

using TaskFlow.MobileApp.ViewModels;

namespace TaskFlow.MobileApp.Views;

public partial class MyJobsPage : ContentPage
{
    private readonly MyJobsViewModel _viewModel;

    public MyJobsPage(MyJobsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// This method is called every time the page is about to be displayed on the screen.
    /// It's the perfect place to load data.
    /// </summary>
	protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Execute the command to fetch the jobs from the API.
        await _viewModel.GetJobsCommand.ExecuteAsync(null);
    }
}
