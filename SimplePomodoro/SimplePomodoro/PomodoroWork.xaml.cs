using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
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
        private object _lock = new object();

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

            Task.Run(StartPomodoro);
            //Task.Run(() =>
            //{
            //    DoWork();
            //});
        }

        private void DoWork()
        {
            var workTime = TimeSpan.FromSeconds(_timeLeftOfWork);
            var breakTime = TimeSpan.FromSeconds(_timeLeftOfBreak);

            var intervals = _intervals;

            ViewModel.WorkNow = true;
            ViewModel.BreakNow = false;

            //for (int i = 0; i < _intervals; i++)
            //{
            //    ViewModel.WorkNow = false;
            //    ViewModel.BreakNow = true;
            //    await Task.Delay(TimeSpan.FromSeconds(_timeLeftOfWork));
            //    ViewModel.WorkNow = true;
            //    ViewModel.BreakNow = false;
            //}

            Debug.WriteLine($"Work interval number {intervals} started {DateTime.Now.ToString("HH mm ss")}");
            Device.StartTimer(TimeSpan.FromSeconds(_timeLeftOfWork), () =>
            {
                lock (_lock)
                {
                    ViewModel.WorkNow = false;
                    ViewModel.BreakNow = true;
                    Debug.WriteLine($"Break interval number {intervals} started {DateTime.Now.ToString("HH mm ss")}");
                    Task.Delay(TimeSpan.FromSeconds(_timeLeftOfWork));
                    Task.WaitAny();
                    Debug.WriteLine($"Work interval number {intervals} started {DateTime.Now.ToString("HH mm ss")}");
                    ViewModel.WorkNow = true;
                    ViewModel.BreakNow = false;
                    //Device.StartTimer(TimeSpan.FromSeconds(_timeLeftOfBreak), () =>
                    //{
                    //    ViewModel.WorkNow = true;
                    //    ViewModel.BreakNow = false;
                    //    Debug.WriteLine($"Break interval number {intervals} ended {DateTime.Now.ToString("HH mm ss")}");

                    //    return true;
                    //});

                    intervals--;
                    if (intervals > 0)
                        return true;

                    Debug.WriteLine($"WORK timer ended {DateTime.Now.ToString("HH mm ss")}");
                    return false;
                }
            });
            
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

        private void DoBreak()
        {
            var time = TimeSpan.FromSeconds(_timeLeftOfBreak);

            Device.StartTimer(time, () =>
            {

                Debug.WriteLine($"Break intervval number {_intervals} ended");
                //DoWork();
                return false;
            });
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