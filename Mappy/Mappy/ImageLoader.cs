using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mappy
{
    public class ImageLoader
    {
#if __IOS__
        public static string resourcePrefix = "Mappy.iOS."
#elif __ANDROID__
        public static string resourcePrefix = "Mappy.Android."
#elif WINDOWS_UWP
        public static string resourcePrefix = "Mappy.UWP."
#else
        public static string resourcePrefix = "Mappy.";
#endif

        public ImageLoader()
        {
            images = new Dictionary<string, SKImage>();
            assembly = IntrospectionExtensions.GetTypeInfo(this.GetType()).Assembly;
        }

        public Dictionary<string, SKImage> images;

        Assembly assembly;

        public SKImage Load(string path)
        {
            SKImage image = null;

            path = (resourcePrefix + "Images." + path);

            if (images.TryGetValue(path, out image))
            {
                return image;
            }
            
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (var managedStream = new SKManagedStream(stream))
            {
                image = SKImage.FromBitmap(SKBitmap.Decode(managedStream));
            }

            images.Add(path, image);

            return image;
        }
    }
}
