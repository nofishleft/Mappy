using Expandable;
using Mappy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Mappy.Views
{
    public class ObjectView : ContentView
    {
        public Object Object { get; set; }
        public List<ObjectView> children;

        public ObjectView(Object @object)
        {
            Object = @object;

            if (Object.Children != null)
            {
                children = new List<ObjectView>();

                StackLayout childrenView;
                ExpandableView v = new ExpandableView()
                {
                    PrimaryView = CreatePrimaryView(),

                    SecondaryView = childrenView = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                    },
                };

                v.TouchHandlerView = v.PrimaryView;

                foreach (var child in Object.Children)
                {
                    ObjectView objectView = new ObjectView(child);
                    children.Add(objectView);
                    childrenView.Children.Add(objectView);
                }

                Content = v;
            }
            else
            {
                children = null;
                Content = CreatePrimaryView();
            }
        }

        private Grid CreatePrimaryView()
        {
            Label lbl;
            CheckBox chkbox;
            Grid grid = new Grid
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition
                        {
                            Width = new GridLength(5, GridUnitType.Star)
                        },
                        new ColumnDefinition
                        {
                            Width = new GridLength(1, GridUnitType.Star)
                        }
                    },
                Children =
                    {
                        (lbl = new Label
                        {
                            Text = Object.Name,
                            HorizontalTextAlignment = TextAlignment.Start,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.Gray,
                            Opacity = 1
                        }),

                        (chkbox = new CheckBox()
                        {
                            IsChecked = Object.Visible,
                            Color = Color.Gray,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Opacity = 1
                        })

                    }
            };

            Grid.SetColumn(lbl, 0);
            Grid.SetColumn(chkbox, 1);

            chkbox.CheckedChanged += CheckBox_CheckedChanged;

            return grid;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Object.Visible = e.Value;
            MapPage.UpdateMapImage();
        }

        private async void EditButtonClicked(object sender, ClickedEventArgs e)
        {
            string task = await App.app.MainPage.DisplayActionSheet(title: $"Edit {Object.Name}", cancel: "Cancel", destruction: "Delete", buttons: new string[] { "Change Name" });

            switch (task)
            {
                case "Change Name":
                    ChangeName();
                    break;
                case "Delete":
                    Content = null;
                    IsVisible = false;
                    IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private async void ChangeName()
        {
            string name = await App.app.MainPage.DisplayPromptAsync(title: "Change Name", message: $"Change {Object.Name} to:", accept: "Save", cancel: "Cancel", placeholder: Object.Name, keyboard: Keyboard.Plain);
            if (name != null) Object.Name = name;
        }
    }
}