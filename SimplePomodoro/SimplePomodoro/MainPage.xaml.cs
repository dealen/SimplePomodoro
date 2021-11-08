using SimplePomodoro.ViewModels;
using SimplePomodoro.ViewModels.Base;
using System;
using Xamarin.Forms;

namespace SimplePomodoro
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
