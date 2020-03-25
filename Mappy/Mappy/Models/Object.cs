using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using SkiaSharp;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace Mappy.Models
{
    [JsonObject]
    public class Object
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        [JsonProperty]
        public string Image { get; set; }

        [JsonIgnore]
        public SKImage ImageData;

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(Resize.Fit)]
        public Resize ResizeMethod { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(Alignment.Center)]
        public Alignment HorizontalAlignment { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(Alignment.Center)]
        public Alignment VerticalAlignment { get; set; }

        [JsonProperty]
        public List<Object> Children { get; set; }

        [JsonIgnore]
        public static double destWidth = 0;
        [JsonIgnore]
        public static double destHeight = 0;

        [JsonIgnore]
        public SKRect rect;

        public SKRect FindSize()
        {
            int w = ImageData.Width;
            int h = ImageData.Height;

            float i_ar = w / ((float)h);
            float d_ar = (float)(destWidth / destHeight);

            float finalWidth = (float)destWidth;
            float finalHeight = (float)destHeight;

            switch (ResizeMethod)
            {
                case Resize.Stretch:
                    break;
                case Resize.Fill:
                    if (i_ar > d_ar)
                    {
                        finalWidth *= i_ar / d_ar;
                    }
                    else if (d_ar > i_ar)
                    {
                        finalHeight *= d_ar / i_ar;
                    }
                    break;
                case Resize.Fit:
                default:
                    if (i_ar > d_ar)
                    {
                        finalHeight *= d_ar / i_ar;
                    }
                    else if (d_ar > i_ar)
                    {
                        finalWidth *= i_ar / d_ar;
                    }
                    break;
            }

            return new SKRect { Left = 0, Right = finalWidth, Top = 0, Bottom = finalHeight };
        }

        public SKRect AdjustByAlignment(SKRect rect)
        {

            float shift = 0;
            switch (HorizontalAlignment)
            {
                case Alignment.Start:
                    shift = ((float)destWidth) - rect.Right;
                    break;
                case Alignment.Center:
                    shift = (((float)destWidth) - rect.Right) / 2;
                    break;
                case Alignment.End:
                default:
                    break;
            }
            rect.Right += shift;
            rect.Left += shift;

            shift = 0;
            switch (VerticalAlignment)
            {
                case Alignment.Start:
                    shift = ((float)destHeight) - rect.Bottom;
                    break;
                case Alignment.Center:
                    shift = (((float)destHeight) - rect.Bottom) / 2;
                    break;
                case Alignment.End:
                default:
                    break;
            }
            rect.Top += shift;
            rect.Bottom += shift;

            return rect;
        }

        public void Draw(SKImageInfo info, SKCanvas canvas)
        {
            if (Visible)
            {
                if (ImageData != null) canvas.DrawImage(ImageData, AdjustRect(info));

                if (Children != null)
                    foreach (var o in Children)
                    {
                        o.Draw(info, canvas);
                    }
            }
        }

        private SKRect AdjustRect(SKImageInfo info)
        {
            SKRect r = rect;

            r.Left = (float)(r.Left * info.Width / destWidth);
            r.Right = (float)(r.Right * info.Width / destWidth);

            r.Top = (float)(r.Top * info.Height / destHeight);
            r.Bottom = (float)(r.Bottom * info.Height / destHeight);

            return r;
        }

        public void UpdateRect()
        {
            if (ImageData != null) rect = AdjustByAlignment(FindSize());

            if (Children != null)
                foreach (Object o in Children)
                {
                    o.UpdateRect();
                }
        }

        public void LoadImage(ImageLoader l)
        {
            if (Image != null && Image != "") ImageData = l.Load(Image);

            if (Children != null)
                foreach (var o in Children)
                {
                    o.LoadImage(l);
                }
        }

        public void Map(ListView l)
        {
            l.ItemsSource = Children;
        }
    }

    public enum Resize
    {
        Stretch,
        Fill,
        Fit
    }

    public enum Alignment
    {
        Start,
        Center,
        End
    }
}
