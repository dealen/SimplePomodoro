using ReactiveUI;
using SimplePomodoro.DataAccess;
using SimplePomodoro.DataAccess.Model;
using SimplePomodoro.Helpers;
using SimplePomodoro.Localization;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimplePomodoro.ViewModels
{
    public class MainPageViewModel : ReactiveObject
    {
        private readonly PomodoroRepository _pomodoroRepository;
        private int _intervals;
        private string _name;
        private ObservableCollection<Schedule> _schdules;
        private Schedule _selectedSchedule;
        private int _timeLeftForBreak;
        private int _timeOfWork;
        private TimeUnits _timeUnit;
        private Color _selectedColorForWork;
        private Color _selectedColorForBreak;

        public MainPageViewModel()
        {
            _pomodoroRepository = new PomodoroRepository();
            InitSchedule(null);
            CommandAddEntrySchedule = new Command(AddToSchedule);
            CommandDeleteSchedule = new Command(DeleteSchedule);
            CommandShowSettings = new Command(ShowSettings);

            ConfigureInteraction();
        }

        private void ConfigureInteraction()
        {
            this.WhenAnyValue(
                tow => tow.TimeOfWork,
                tlfb => tlfb.TimeLeftForBreak,
                name => name.Name,
                i => i.Intervals)
                .Subscribe(item =>
                {
                    this.RaisePropertyChanged(nameof(IsStartEnabled));
                });

            this.WhenAnyValue(s => s.SelectedSchedule)
                .Subscribe(schedule =>
                {
                    _OnSelectedScheduleChanged();
                });
        }

        public ICommand CommandAddEntrySchedule { get; }

        public ICommand CommandDeleteSchedule { get; }

        public ICommand CommandShowSettings { get; }

        public int Intervals
        {
            get => _intervals;
            set
            {
                this.RaiseAndSetIfChanged(ref _intervals, value);
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

        public string Name
        {
            get => _name;
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);
            }
        }

        public ObservableCollection<Schedule> Schdules
        {
            get => _schdules;
            set => this.RaiseAndSetIfChanged(ref _schdules, value);
        }

        public Schedule SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSchedule, value);
            }
        }

        public int TimeLeftForBreak
        {
            get => _timeLeftForBreak;
            set
            {
                this.RaiseAndSetIfChanged(ref _timeLeftForBreak, value);
            }
        }

        public int TimeOfWork
        {
            get => _timeOfWork;
            set
            {
                this.RaiseAndSetIfChanged(ref _timeOfWork, value);
            }
        }

        public TimeUnits TimeUnit
        {
            get => _timeUnit;
            set => this.RaiseAndSetIfChanged(ref _timeUnit, value);
        }

        public Color SelectedColorForWork
        {
            get => _selectedColorForWork;
            set => this.RaiseAndSetIfChanged(ref _selectedColorForWork, value);
        }

        public Color SelectedColorForBreak
        {
            get => _selectedColorForBreak;
            set => this.RaiseAndSetIfChanged(ref _selectedColorForBreak, value);
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
                        await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.NameEmpty, Language.OK);
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

        private void ShowSettings(object obj)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
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

        private async Task<bool> Verify()
        {
            if (TimeOfWork <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.TimeLeftOfWorkLessThanZero, Language.OK);
                return false;
            }

            if (TimeLeftForBreak <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.TimeForBreakLessThanZero, Language.OK);
                return false;
            }

            if (Intervals <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Language.Verification, Language.IntervalsLessThanZero, Language.OK);
                return false;
            }

            return true;
        }
    }
}