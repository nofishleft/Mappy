using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mappy.Views
{
    public class MainPage : ContentPage
    {
        ListView sideView;
        Frame mainView;
        public MainPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };

            sideView = new ListView();
            mainView = new Frame();

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