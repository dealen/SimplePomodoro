using SimplePomodoro.Helpers;
using SimplePomodoro.ViewModels.Base;
using System;

namespace SimplePomodoro.ViewModels
{
    public class PomodoroWorkViewModel : BaseViewModel
    {
        private bool _workNow;
        private bool _breakNow;
        private int _timeLeftOfWork;
        private int _timeLeftOfBreak;
        private int _totalIntervals;
        private TimeUnits _timeUnits;

        internal void SetTimeOfBreakAndWork(int workTime, int breakTime, TimeUnits timeUnits, int intervals)
        {
            _timeUnits = timeUnits;
            _totalIntervals = intervals;
            switch (_timeUnits)
            {
                case TimeUnits.Second:
                    _timeLeftOfBreak = breakTime;
                    _timeLeftOfWork = workTime;
                    break;
                case TimeUnits.Minutes:
                    _timeLeftOfBreak = breakTime * 60;
                    _timeLeftOfWork = workTime * 60;
                    break;
                case TimeUnits.Hours:
                    _timeLeftOfBreak = breakTime * 60 * 60;
                    _timeLeftOfWork = workTime * 60 * 60;
                    break;
                default:
                    break;
            }
        }

        public int Intervals
        {
            get { return _totalIntervals; }
            private set { SetField(ref _totalIntervals, value); }
        }

        public TimeUnits TimeUnits
        {
            get { return _timeUnits; }
            private set { SetField(ref _timeUnits, value); }
        }

        public int TimeLeftOfWork
        {
            get { return _timeLeftOfWork; }
            private set { SetField(ref _timeLeftOfWork, value); }
        }

        internal int TakeOneInterval()
        {
            return _totalIntervals--;
        }

        public int TimeLeftOfBreak
        {
            get { return _timeLeftOfBreak; }
            private set { SetField(ref _timeLeftOfBreak, value); }
        }

        public bool WorkNow
        {
            get { return _workNow; }
            set { SetField(ref _workNow, value); }
        }

        public bool BreakNow
        {
            get { return _breakNow; }
            set { SetField(ref _breakNow, value); }
        }
    }
}
