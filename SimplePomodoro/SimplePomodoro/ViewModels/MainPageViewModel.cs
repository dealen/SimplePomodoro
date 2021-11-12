using SimplePomodoro.DataAccess;
using SimplePomodoro.DataAccess.Model;
using SimplePomodoro.Helpers;
using SimplePomodoro.Localization;
using SimplePomodoro.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private Schedule _selectedSchedule;
        private readonly PomodoroRepository _pomodoroRepository;

        public MainPageViewModel()
        {
            _pomodoroRepository = new PomodoroRepository();
            InitSchedule();
            CommandAddEntrySchedule = new Command(AddToSchedule);
        }

        private void InitSchedule()
        {
            var _schedules = _pomodoroRepository.GetSchedules();
            Schdules = new ObservableCollection<Schedule>(_schedules);
        }

        public Schedule SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                if (SetField(ref _selectedSchedule, value))
                    _OnSelectedScheduleChanged();
            }
        }

        public ICommand CommandAddEntrySchedule { get; }

        public ObservableCollection<Schedule> Schdules { get; set; }

        public int TimeOfWork
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

        public bool IsStartEnabled
        {
            get
            {
                if (TimeOfWork <= 0)
                    return false;

                if (TimeLeftForBreak <= 0)
                    return false;

                if (Intervals > 0)
                    return false;

                if (string.IsNullOrWhiteSpace(Name))
                    return false;

                return true;
            }
        }

        private void _OnSelectedScheduleChanged()
        {
            if (_selectedSchedule == null) return;

            Intervals = _selectedSchedule.Intervals;
            TimeLeftForBreak = _selectedSchedule.TimeOfBreak;
            TimeOfWork = _selectedSchedule.TimeOfWork;
            TimeUnit = (TimeUnits)_selectedSchedule.TimeUnit;
            Name = _selectedSchedule.Name;
        }

        private async void AddToSchedule(object obj)
        {
            try
            {
                if (await Verify())
                {
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.NameEmpty, "Ok");
                        return;
                    }

                    await _pomodoroRepository.AddSchedule(Name, TimeOfWork, TimeLeftForBreak, Intervals, (int)TimeUnit);
                    InitSchedule();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private async Task<bool> Verify()
        {
            if (TimeOfWork <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.TimeLeftOfWorkLessThanZero, "Ok");
                return false;
            }

            if (TimeLeftForBreak <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.TimeForBreakLessThanZero, "Ok");
                return false;
            }

            if (Intervals <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.IntervalsLessThanZero, "Ok");
                return false;
            }

            return true;
        }
    }
}