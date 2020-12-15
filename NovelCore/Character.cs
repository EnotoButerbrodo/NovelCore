using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using System.Windows;
using Point = System.Drawing.Point;



namespace NovelCore
{
    public class Character
    {
        public Character(string name)
        {
            Name = name;
            Appearance = new Image[3];
            Spot = new Grid();
            Spot.RenderTransformOrigin = new System.Windows.Point(0.5, 1);
            Canvas.SetLeft(Spot, 0);
            for (byte i = 0; i < 3; i++)
            {
                Appearance[i] = new Image();
                Appearance[i].Stretch = Stretch.Uniform;
                Appearance[i].RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                Spot.Children.Add(Appearance[i]);
            }
            Appearance[0].Margin = new Thickness(0, 1, 0, 0);
            Sprites = new Dictionary<string, BitmapImage>();

        }
        public string Name { get; set; }
        Image[] Appearance { get; set; }

        Dictionary<string, BitmapImage> Sprites;
        public Grid Spot { get;private set; }// Все картинки персонажа прикреплены сюда
        public Point Position { get; set; }
        Canvas Scene { get; set; } // Сцена, на которой будет персонаж
        
        public void Scale(double x, double y)
        {
            ScaleTransform scale = new ScaleTransform(x, y);
            Spot.RenderTransform = scale;
        }
        
        public void EnterTheScene(Canvas scene)
        {
            Scene = scene;
            if (!Scene.Children.Contains(Spot))
            {
                Spot.Width = scene.ActualWidth;
                Spot.Height = scene.ActualHeight;
                Scene.Children.Add(Spot);
            }
        }
        public void LeaveTheScene(Canvas scene)
        {
            if (scene.Children.Contains(Spot))
                scene.Children.Remove(Spot);
            Scene = null;
        }
        public bool SpriteInCollection(string name)
        {
            return Sprites.ContainsKey(name);
        }
        public void AddSprite(string name, BitmapImage image)
        {
            Sprites.Add(name, image);
        }
        public void RemoveSprite(string name)
        {
            Sprites.Remove(name);
        }
        public void SetAppearance(string[] sprite)
        {
            Appearance[0].Source = Sprites[sprite[0]];

            if (sprite[1] != null)
                Appearance[1].Source = Sprites[sprite[1]];
            else
                Appearance[1].Source = new BitmapImage();
            if (sprite[2] != null)
                Appearance[2].Source = Sprites[sprite[2]];
            else
                Appearance[2].Source = new BitmapImage();
        }

    }

}
