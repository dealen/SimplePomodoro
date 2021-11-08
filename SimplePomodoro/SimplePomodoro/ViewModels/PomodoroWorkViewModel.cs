using SimplePomodoro.ViewModels.Base;

namespace SimplePomodoro.ViewModels
{
    public class PomodoroWorkViewModel : BaseViewModel
    {
        private bool _workNow;
        private bool _breakNow;

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
