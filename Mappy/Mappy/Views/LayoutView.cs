using Mappy.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

namespace Mappy.Views
{
    public class LayoutView : ContentView
    {
        MainView mainView;
        public LayoutMeta meta = null;
        public string layoutFile;

        public LayoutView(string layoutFile, MainView mainView)
        {
            this.mainView = mainView;
            this.layoutFile = layoutFile;

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                },

                Orientation = StackOrientation.Vertical
            };

            TapGestureRecognizer tap = new TapGestureRecognizer();

            tap.Tapped += Tapped;

            this.GestureRecognizers.Add(tap);
        }

        public void Tapped(object sender, EventArgs e)
        {
            // Dynamically load metadata as needed
            if (meta == null) LoadMeta();

            mainView.Select(this);
        }

        public void Highlight()
        {

        }

        public void UnHighlight()
        {

        }

        public void LoadMeta()
        {
            string path = (ImageLoader.resourcePrefix + "LayoutMeta." + layoutFile);

            string contents = null;
            var assembly = IntrospectionExtensions.GetTypeInfo(this.GetType()).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader r = new StreamReader(stream))
                contents = r.ReadToEnd();

            var set = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate };

            meta = JsonConvert.DeserializeObject<LayoutMeta>(contents, set);
        }
    }
}