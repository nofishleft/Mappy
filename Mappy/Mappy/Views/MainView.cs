using Mappy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mappy.Views
{
    public class MainView : ContentView
    {
        public LayoutView Selected { get; private set; }
        StackLayout stack;

        public MainView()
        {
            Content = stack = new StackLayout
            {};
        }

        public void Select(LayoutView newView)
        {
            if (newView == Selected || newView == null) return;

            Selected.UnHighlight();

            stack.Children.Clear();

            LayoutMeta meta = newView.meta;

            Label title = new Label
            {
                Text = meta.Name
            };

            Label author = new Label
            {
                Text = meta.Author
            };

            Label map = new Label
            {
                Text = meta.Map
            };

            Button btn = new Button
            {
                Text = "Open"
            };

            btn.Clicked += OpenButtonClicked;

            newView.Highlight();

            Selected = newView;
        }

        public void OpenButtonClicked(object sender, EventArgs args)
        {
            App.app.LoadMap(Selected.meta.Map);
        }
    }
}