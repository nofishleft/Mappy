using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mappy.Views;

namespace Mappy
{
    public partial class App : Application
    {
        public static App app;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
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
    }
}
