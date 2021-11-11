using SimplePomodoro.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimplePomodoro
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DisplayAlert("Current culture", $"Current culture: {CultureInfo.CurrentUICulture.Name}", "Ok");
        }

        public MainPageViewModel ViewModel
        {
            get => BindingContext as MainPageViewModel;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pomodoroPage = new PomodoroWork(ViewModel.TimeUnit, ViewModel.TimeLeftOfWOrk, ViewModel.TimeLeftForBreak, ViewModel.Intervals);
                await Navigation.PushAsync(pomodoroPage, true);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
