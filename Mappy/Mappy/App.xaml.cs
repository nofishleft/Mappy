using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mappy.Views;
using System.Collections.Generic;

namespace Mappy
{
    public partial class App : Application
    {
        public static App app;

        private MainPage Menu;

        private Dictionary<string, Page> pages;

        public App()
        {
            InitializeComponent();

            MainPage = Menu = new MainPage();
        }

        public void LoadMap(string file)
        {
            if (app.pages.TryGetValue(file, out Page pg))
            {
                app.MainPage = pg;
                return;
            }

            pg = new MapPage(file);

            app.pages.Add(file, pg);

            app.MainPage = pg;
        }

        public void BackToMainMenu()
        {
            MainPage = Menu;
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
