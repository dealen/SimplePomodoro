using SimplePomodoro.ViewModels;
using Xamarin.Forms.Xaml;

namespace SimplePomodoro.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : Rg.Plugins.Popup.Pages.PopupPage
    {
        /*
         * https://github.com/rotorgames/Rg.Plugins.Popup/wiki/PopupPage
         */
        public SettingsView()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}