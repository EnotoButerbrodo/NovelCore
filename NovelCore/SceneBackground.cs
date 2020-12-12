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
    static class SceneBackground
    {
        static SceneBackground()
        {
            Canvas.SetLeft(Image, 0);
            Canvas.SetLeft(Spot, 0);
            Spot.Children.Add(Image);
            Image.Stretch = Stretch.Uniform;
            

        }
        static Image Image { get; set; } = new Image();
        static Grid Spot { get; set; } = new Grid();
        public static void Setup(Canvas scene)
        {
            Panel.SetZIndex(Spot, -1);
            scene.Children.Add(Spot);
            Image.RenderTransformOrigin = new Point(0.5, 0.5);
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
        public static void Move(AnimationSettings args)
        {
            if (args.StartPoint.X != args.EndPoint.X)
            {
                DoubleAnimation anim_X = new DoubleAnimation(args.EndPoint.X, TimeSpan.FromMilliseconds(args.Speed));
                Storyboard.SetTarget(anim_X, Spot);
                Storyboard.SetTargetProperty(anim_X, new PropertyPath("(Canvas.Left)"));
                //SceneAnimation.Children.Add(anim_X);
            }
            if (args.StartPoint.Y != args.EndPoint.Y)
            {
                DoubleAnimation anim_Y = new DoubleAnimation(args.EndPoint.Y, TimeSpan.FromMilliseconds(args.Speed));
                Spot.BeginAnimation(Canvas.LeftProperty, anim_Y);
            }
            //DoubleAnimation anim_X = new DoubleAnimation(x, TimeSpan.FromMilliseconds(500));
            //DoubleAnimation anim_Y = new DoubleAnimation(y, TimeSpan.FromMilliseconds(500));
            //Image.RenderTransformOrigin,
            //Image.BeginAnimation(Image.RenderTransformOriginProperty, anim_Y);
            //Image.RenderTransformOrigin = new Point(x, y);

            //Canvas.SetLeft(Image, x);
            //Canvas.SetBottom(Image, y);
        } 
    }
}
