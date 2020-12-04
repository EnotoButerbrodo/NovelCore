﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;



namespace NovelCore
{
    public class Character
    {
        public Character(string name)
        {
            Name = name;
            Appearance = new Image[3];
            Spot = new Grid();
            for(byte i = 0;i < 3; i++)
            {
                Appearance[i] = new Image();
                Appearance[i].Stretch = Stretch.Uniform;
                Spot.Children.Add(Appearance[i]);
            }
            Sprites = new Dictionary<string, BitmapImage>();
            
        }
        public string Name { get; set; }
        Image[] Appearance { get; set; }

        Dictionary<string, BitmapImage> Sprites;
        Grid Spot { get; set; }// Все картинки персонажа прикреплены сюда
        Point Position { get; set; }
        Canvas Scene { get; set; } // Сцена, на которой будет персонаж

        public event Action<AnimationEventArgs> Animation;
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
        public void LeaveTheScene()
        {
            if (Scene.Children.Contains(Spot))
                Scene.Children.Remove(Spot);
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

        public void DoAnimation(ref AnimationEventArgs args)
        {
            if(Position!= args.point)
            {
                Animation?.Invoke(args);
                Position = args.point;
            }
        }
    }

}