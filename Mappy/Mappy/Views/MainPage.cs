using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mappy.Views
{
    public class MainPage : ContentPage
    {
        ScrollView sideView;
        MainView mainView;
        public MainPage()
        {
            mainView = new MainView();

            sideView = new ScrollView()
            {
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,

                    Children =
                    {
                        new LayoutView("reserve.json", mainView)
                    }
                }
            };

            Content = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width= new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width= new GridLength(1, GridUnitType.Star) }
                }
            };

            ((Grid)Content).Children.Add(sideView, 0, 0);
            ((Grid)Content).Children.Add(mainView, 1, 0);
        }
    }
}