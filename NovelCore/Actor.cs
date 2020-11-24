
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace NovelCore
{
    public class Actor
    {
        public Actor(string name, SolidColorBrush color, ref Canvas scene)
        {
            Name = name;
            Color = color;
            Appearance = new Image[3];
            Spot = new Grid();
            for(byte i = 0;i < 3; i++)
            {
                Appearance[i] = new Image();
                Appearance[i].Stretch = Stretch.Fill;
                Spot.Children.Add(Appearance[i]);
            }
            Scene = scene;
            
        }
        public string Name { get; private set; }
        public SolidColorBrush Color { get; private set; }
        string Speech { get; set; }
        Image[] Appearance { get; set; }
        Grid Spot { get; set; }
        Point Position { get; set; }
        Canvas Scene { get; set; }

        public void EnterTheScene()
        {
            Scene.Children.Add(Spot);
        }
        public void LeaveTheScene()
        {
            Scene.Children.Remove(Spot);
        }
        public void SetSpeech(string text)
        {
            Speech = text;
        }
        public void SetAppearance(BitmapImage emotionImage,
            BitmapImage bodyLeftImage,
            BitmapImage bodyRightImage)
        {
            Appearance[0].Source = emotionImage;
            Appearance[1].Source = bodyLeftImage;
            Appearance[2].Source = bodyRightImage;
        }

        public void SetPosition(Point point)
        {
            if(Position!= point)
            {
                Position = point;
                Canvas.SetLeft(Scene, point.X);
                Canvas.SetBottom(Scene, point.Y);
            }
        }
    }
}
