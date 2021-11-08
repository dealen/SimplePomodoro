using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels;
using System;
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
        private readonly int _intervals;

        public PomodoroWorkViewModel ViewModel
        {
            get
            {
                return BindingContext as PomodoroWorkViewModel;
            }
        }

        public PomodoroWork(TimeUnits timeUnit, int timeLeftOfWork, int timeLeftOfBreak, int intervals)
        {
            InitializeComponent();

            _timeUnits = timeUnit;
            switch (_timeUnits)
            {
                case TimeUnits.Second:
                    _timeLeftOfBreak = timeLeftOfBreak;
                    _timeLeftOfWork = timeLeftOfWork;
                    _intervals = intervals;
                    break;
                case TimeUnits.Minutes:
                    _timeLeftOfBreak = timeLeftOfBreak * 60;
                    _timeLeftOfWork = timeLeftOfWork * 60;
                    _intervals = intervals * 60;
                    break;
                case TimeUnits.Hours:
                    _timeLeftOfBreak = timeLeftOfBreak * 60 * 60;
                    _timeLeftOfWork = timeLeftOfWork * 60 * 60;
                    _intervals = intervals * 60 * 60;
                    break;
                default:
                    break;
            }

            Task.Run(StartPomodoro);
        }

        private async void StartPomodoro()
        {
            for (int i = 0; i < _intervals; i++)
            {
                var startOfInterval = DateTime.Now;

                var isWorkCompleted = false;
                var isBreakCompleted = false;

                try
                {
                    ViewModel.WorkNow = true;
                    while (!isWorkCompleted)
                    {
                        if (startOfInterval.AddSeconds(_timeLeftOfWork) < DateTime.Now)
                        {
                            isWorkCompleted = true;
                            ViewModel.WorkNow = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                try
                {
                    var startOfBreak = DateTime.Now;
                    ViewModel.BreakNow = true;
                    while (!isBreakCompleted)
                    {
                        if (startOfBreak.AddSeconds(_timeLeftOfBreak) < DateTime.Now)
                        {
                            isBreakCompleted = true;
                            ViewModel.BreakNow = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Work completed", "Your work is completed", "Ok");

                    Navigation.PopToRootAsync(true);
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync(true);
        }
    }
}