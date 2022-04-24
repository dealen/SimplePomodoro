using SimplePomodoro.ViewModels.Base;
using Xamarin.Forms;

namespace SimplePomodoro.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Color SelectedColorForWork { get; set; }
        public Color SelectedColorForBreak { get; set; }
    }
}
