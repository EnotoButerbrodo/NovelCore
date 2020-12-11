using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
namespace NovelCore
{
    static class SceneBackground
    {
        static SceneBackground()
        {
            Canvas.SetLeft(Image, 0);
            Spot.Children.Add(Image);
            Image.Stretch = Stretch.Uniform;
            

        }
        static Image Image { get; set; } = new Image();
        static Grid Spot { get; set; } = new Grid();
        public static void Setup(Canvas scene)
        {
            Panel.SetZIndex(Spot, -1);
            scene.Children.Add(Spot);
        }
        public static void Scale(double x, double y)
        {
            ScaleTransform scale = new ScaleTransform(x, y);
            Image.RenderTransform = scale;
        }
        public static void SetImage(BitmapImage image)
        {
            Image.Source = image;
        }
        public static void Move(double x, double y)
        {
            Image.RenderTransformOrigin = new Point(x, y);
            //Canvas.SetLeft(Image, x);
            //Canvas.SetBottom(Image, y);
        } 
    }
}
