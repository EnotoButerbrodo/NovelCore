using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NovelCore
{
    static class SceneBackground
    {
        static SceneBackground()
        {
            Canvas.SetLeft(Image, 0);
        }
        static Image Image { get; set; } = new Image();
        public static void Setup(Canvas scene)
        {
            Panel.SetZIndex(Image, -1);
            Image.Width = scene.Width;
            Image.Height = scene.Height;
            scene.Children.Add(Image);
        }
        public static void SetImage(BitmapImage image)
        {
            Image.Source = image;
        }
        public static void Move(double x, double y)
        {
            Canvas.SetLeft(Image, x);
            Canvas.SetBottom(Image, y);
        } 
    }
}
