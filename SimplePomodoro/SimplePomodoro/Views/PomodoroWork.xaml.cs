using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimplePomodoro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PomodoroWork : ContentPage
    {
        private event Action Work;
        private event Action Break;

        public PomodoroWorkViewModel ViewModel => BindingContext as PomodoroWorkViewModel;

        public PomodoroWork(TimeUnits timeUnit, int timeLeftOfWork, int timeLeftOfBreak, int intervals)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            ViewModel.SetTimeOfBreakAndWork(timeLeftOfWork, timeLeftOfBreak, timeUnit, intervals);

            Work -= PomodoroWork_Work;
            Work += PomodoroWork_Work;
            Break -= PomodoroWork_Break;
            Break += PomodoroWork_Break;

            Task.Run(() => Start());
        }

        private void Start()
        {
            Work?.Invoke();
        }

        private void PomodoroWork_Work()
        {
            ViewModel.WorkNow = true;
            ViewModel.BreakNow = false;
            Device.StartTimer(TimeSpan.FromSeconds(ViewModel.TimeLeftOfWork), () =>
            {
                Debug.WriteLine($"Work interval ended");
                Break?.Invoke();
                return false;
            });
        }

        private void PomodoroWork_Break()
        {
            ViewModel.WorkNow = false;
            ViewModel.BreakNow = true;
            if (ViewModel.TakeOneInterval() > 0)
            {
                Device.StartTimer(TimeSpan.FromSeconds(ViewModel.TimeLeftOfBreak), () =>
                {
                    Debug.WriteLine($"Break interval ended");
                    Work?.Invoke();
                    return false;
                });
            }
            else
            {
                Work = null;
                Break = null;
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Work completed", "Your work is completed", "Ok");

                    Work -= PomodoroWork_Work;
                    Break -= PomodoroWork_Break;
                    Navigation.PopToRootAsync(true);
                });
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync(true);
        }
    }
}