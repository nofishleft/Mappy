using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

using Mappy.Models;

namespace Mappy.Views
{
    public class ObjectListView : ContentView
    {
        public ObjectListView(List<Object> root)
        {
            StackLayout l;
            Content = new ScrollView
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,

                Content = l = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                }
            };

            CreateObjectListView(l.Children, root);
        }

        public void CreateObjectListView(IList<View> il, List<Object> root)
        {
            foreach (var o in root)
            {
                il.Add(new ObjectView(o));
            }
        }
    }
}