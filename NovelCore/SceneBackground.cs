using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;

namespace NovelCore
{
    public class SceneImage
    {
        public SceneImage()
        {
            Image.Stretch = Stretch.Uniform;
            Image.RenderTransformOrigin = new Point(0.5, 0.5);
            Canvas.SetLeft(Spot, 0);
            Canvas.SetTop(Spot, 0);
            Spot.Children.Add(Image);
            SetZ(-1);
        }
        public Image Image { get; set; } = new Image();
        public Grid Spot { get; set; } = new Grid();
        public void SetImage(BitmapImage image)
        {
            Image.Source = image;
        }
        public void Scale(double x, double y)
        {     
            ScaleTransform scale = new ScaleTransform(x, y);
            Image.RenderTransform = scale;
        }
        public void SetZ(int Z)
        {
            Panel.SetZIndex(Spot, Z);
        }
    }
}
