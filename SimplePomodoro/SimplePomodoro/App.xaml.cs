using System.Globalization;
using Xamarin.Forms;

namespace SimplePomodoro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetCultureInfo(CultureInfo.GetCultureInfo("pl-PL"));

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static void SetCultureInfo(CultureInfo info)
        {
            CultureInfo.CurrentCulture = info;
            CultureInfo.CurrentUICulture = info;
        }
    }
}
