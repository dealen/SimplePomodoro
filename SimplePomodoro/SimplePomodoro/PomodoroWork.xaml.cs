using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimplePomodoro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PomodoroWork : ContentPage
    {
        private readonly TimeUnits _timeUnits;
        private readonly int _timeLeftOfWork;
        private readonly int _timeLeftOfBreak;
        private int _intervals;

        private event Action Work;
        private event Action Break;

        public PomodoroWorkViewModel ViewModel => BindingContext as PomodoroWorkViewModel;

        public PomodoroWork(TimeUnits timeUnit, int timeLeftOfWork, int timeLeftOfBreak, int intervals)
        {
            InitializeComponent();

            _intervals = intervals;
            _timeUnits = timeUnit;
            switch (_timeUnits)
            {
                case TimeUnits.Second:
                    _timeLeftOfBreak = timeLeftOfBreak;
                    _timeLeftOfWork = timeLeftOfWork;
                    break;
                case TimeUnits.Minutes:
                    _timeLeftOfBreak = timeLeftOfBreak * 60;
                    _timeLeftOfWork = timeLeftOfWork * 60;
                    break;
                case TimeUnits.Hours:
                    _timeLeftOfBreak = timeLeftOfBreak * 60 * 60;
                    _timeLeftOfWork = timeLeftOfWork * 60 * 60;
                    break;
                default:
                    break;
            }

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
            Device.StartTimer(TimeSpan.FromSeconds(_timeLeftOfWork), () =>
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
            _intervals--;
            if (_intervals > 0)
            {
                Device.StartTimer(TimeSpan.FromSeconds(_timeLeftOfBreak), () =>
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