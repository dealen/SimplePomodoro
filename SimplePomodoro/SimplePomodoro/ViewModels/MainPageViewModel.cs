using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels.Base;

namespace SimplePomodoro.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private int _timeLeftOfWOrk;
        private int _timeLeftForBreak;
        private TimeUnits _timeUnit;
        private int _intervals;

        public MainPageViewModel()
        {

        }

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
    }
}
