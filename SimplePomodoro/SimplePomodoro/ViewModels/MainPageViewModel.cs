using SimplePomodoro.DataAccess;
using SimplePomodoro.DataAccess.Model;
using SimplePomodoro.Helpers;
using SimplePomodoro.Localization;
using SimplePomodoro.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimplePomodoro.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private int _timeOfWork;
        private int _timeLeftForBreak;
        private TimeUnits _timeUnit;
        private int _intervals;
        private string _name;
        private Schedule _selectedSchedule;
        private ObservableCollection<Schedule> _schdules;
        private readonly PomodoroRepository _pomodoroRepository;

        public MainPageViewModel()
        {
            _pomodoroRepository = new PomodoroRepository();
            InitSchedule(null);
            CommandAddEntrySchedule = new Command(AddToSchedule);
            CommandDeleteSchedule = new Command(DeleteSchedule);
        }

        private void InitSchedule(Schedule selectedSchedule)
        {
            var _schedules = _pomodoroRepository.GetSchedules();
            Schdules = null;
            Schdules = new ObservableCollection<Schedule>(_schedules);
            if (!_schedules.Any()) return;
            if (selectedSchedule != null)
            {
                var selectedItem = _schedules.FirstOrDefault(x => x.ID.Equals(selectedSchedule.ID));
                if (selectedItem != null)
                    SelectedSchedule = selectedItem;
                else
                    SelectedSchedule = _schedules.First();
            }
            else
            {
                SelectedSchedule = _schedules.First();
            }
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
        public ICommand CommandDeleteSchedule { get; }

        public ObservableCollection<Schedule> Schdules
        {
            get => _schdules;
            set => SetField(ref _schdules, value);
        }

        public int TimeOfWork
        {
            get => _timeOfWork;
            set
            {
                SetField(ref _timeOfWork, value);
                this.OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        public int TimeLeftForBreak
        {
            get => _timeLeftForBreak;
            set
            {
                SetField(ref _timeLeftForBreak, value);
                this.OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        public TimeUnits TimeUnit
        {
            get => _timeUnit;
            set => SetField(ref _timeUnit, value);
        }

        public int Intervals
        {
            get => _intervals;
            set
            {
                SetField(ref _intervals, value);
                this.OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                SetField(ref _name, value);
                this.OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        public bool IsStartEnabled
        {
            get
            {
                if (TimeOfWork <= 0)
                    return false;

                if (TimeLeftForBreak <= 0)
                    return false;

                if (Intervals < 0)
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
            this.OnPropertyChanged(nameof(IsStartEnabled));
        }

        private async void DeleteSchedule(object obj)
        {
            try
            {
                if (obj is Schedule s)
                {
                    await _pomodoroRepository.DeleteSchedule(s.ID);
                    InitSchedule(_selectedSchedule);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
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
                    InitSchedule(_selectedSchedule);
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