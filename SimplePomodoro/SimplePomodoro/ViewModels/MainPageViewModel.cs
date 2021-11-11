using SimplePomodoro.DataAccess;
using SimplePomodoro.DataAccess.Model;
using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimplePomodoro.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private int _timeLeftOfWOrk;
        private int _timeLeftForBreak;
        private TimeUnits _timeUnit;
        private int _intervals;
        private string _name;
        private readonly PomodoroRepository _pomodoroRepository;

        public MainPageViewModel()
        {
            _pomodoroRepository = new PomodoroRepository();
            InitSchedule();
            CommandAddEntrySchedule = new Command(AddToSchedule);
        }

        private void InitSchedule()
        {
            Schdules = new ObservableCollection<Schedule>(_pomodoroRepository.GetSchedules());
        }

        public ICommand CommandAddEntrySchedule { get; }

        public ObservableCollection<Schedule> Schdules { get; set; }

        public int TimeLeftOfWOrk 
        {
            get => _timeLeftOfWOrk;
            set => SetField(ref _timeLeftOfWOrk, value); 
        }

        public int TimeLeftForBreak
        {
            get => _timeLeftForBreak;
            set => SetField(ref _timeLeftForBreak, value);
        }

        public TimeUnits TimeUnit
        {
            get => _timeUnit;
            set => SetField(ref _timeUnit, value);
        }

        public int Intervals
        {
            get => _intervals;
            set => SetField(ref _intervals, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private async void AddToSchedule(object obj)
        {
            try
            {
                await _pomodoroRepository.AddSchedule(Name, TimeLeftOfWOrk, TimeLeftForBreak, Intervals, (int)TimeUnit);
                InitSchedule();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
    }
}
