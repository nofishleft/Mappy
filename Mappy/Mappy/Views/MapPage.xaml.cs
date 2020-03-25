using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Reflection;
using Mappy.Models;

namespace Mappy.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MapPage : ContentPage
    {
        View sidebar;
        Button hidebtn;
        StackLayout mainContent;
        SKColor clearColor;
        SKCanvasView canvasView;
        Frame sidebarList;

        private static MapPage self;
        public static void UpdateMapImage()
        {
            if (self != null)
                self.canvasView.InvalidateSurface();
        }

        List<Object> root;

        public MapPage(string layoutFile)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MapPage)).Assembly;
            string path = $"{ImageLoader.resourcePrefix}Layouts.{layoutFile}";
            Stream stream = assembly.GetManifestResourceStream(path);
            StreamReader r = new StreamReader(stream);

            string contents = r.ReadToEnd();

            r.Close();
            stream.Close();

            LoadData(contents);

            self = this;

            InitializeComponent();
            LoadComponentReferences();

            MapData();
        }

        public void MapData()
        {
            sidebarList.Content = new ObjectListView(root);
        }

        public void LoadData(string contents)
        {
            var set = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate };

            root = JsonConvert.DeserializeObject<List<Object>>(contents, set);

            if (root != null)
            {
                var l = new ImageLoader();
                foreach (var o in root)
                {
                    o.LoadImage(l);
                }
            }
        }

        public void LoadComponentReferences()
        {
            sidebar = this.FindByName<View>("SideBar");
            hidebtn = this.FindByName<Button>("HideSidebarButton");
            mainContent = this.FindByName<StackLayout>("MainContent");
            canvasView = this.FindByName<SKCanvasView>("Canvas");
            sidebarList = this.FindByName<Frame>("SideBarList");
        }

        public void LoadBrushes()
        {
            clearColor = Color.Black.ToSKColor();
            //clearColor = Color.Yellow.ToSKColor();
        }

        public void CloseSidePanel()
        {
            sidebar.IsEnabled = (sidebar.IsVisible = false);
        }

        public void OpenSidePanel()
        {
            sidebar.IsEnabled = (sidebar.IsVisible = true);
        }

        public void ToggleSidePanel()
        {
            sidebar.IsEnabled = (sidebar.IsVisible = !sidebar.IsVisible);
        }

        // Event Function Called By HideSidebarButton
        private void CloseSidePanel(object sender, System.EventArgs e)
        {
            CloseSidePanel();
        }

        // Event Function Called By TapGesture
        private void ToggleSidePanel(object sender, System.EventArgs e)
        {
            ToggleSidePanel();
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(clearColor);

            if (root == null)
            {
                return;
            }

            if (Object.destWidth != canvasView.Width || Object.destHeight != canvasView.Height)
            {
                Object.destWidth = canvasView.Width;
                Object.destHeight = canvasView.Height;

                foreach (var o in root)
                {
                    o.UpdateRect();
                }
            }

            foreach (var o in root)
            {
                o.Draw(info, canvas);
            }
        }

        private void MainMenuButton_Clicked(object sender, System.EventArgs e)
        {
            //App.BackToMainMenu();
        }
    }
}
