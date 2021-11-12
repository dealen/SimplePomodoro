using SimplePomodoro.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace SimplePomodoro
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public MainPageViewModel ViewModel
        {
            get => BindingContext as MainPageViewModel;
        }

        private async void btnStart_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pomodoroPage = new PomodoroWork(ViewModel.TimeUnit, ViewModel.TimeOfWork, ViewModel.TimeLeftForBreak, ViewModel.Intervals);
                await Navigation.PushAsync(pomodoroPage, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            // close
        }
    }
}
