using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;



namespace NovelCore
{
    public class Actor
    {
        public Actor(string name, Canvas scene)
        {
            Name = name;
            Appearance = new Image[3];
            Spot = new Grid();
            for(byte i = 0;i < 3; i++)
            {
                Appearance[i] = new Image();
                Appearance[i].Stretch = Stretch.Fill;
                Spot.Children.Add(Appearance[i]);
            }
            Sprites = new Dictionary<string, BitmapImage>();
            Scene = scene;
            
        }
        public string Name { get; set; }
        Image[] Appearance { get; set; }

        Dictionary<string, BitmapImage> Sprites;
        Grid Spot { get; set; }// Все картинки персонажа прикреплены сюда
        Point Position { get; set; }
        Canvas Scene { get; set; } // Сцена, на которой будет персонаж

        public event Action<AnimationEventArgs> Animation;
        public void EnterTheScene()
        {
            if(!Scene.Children.Contains(Spot))
                Scene.Children.Add(Spot);
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
        public void SetAppearance(string emotionName,
            string bodyLeftName = null,
            string bodyRightName = null)
        {
            Appearance[0].Source = Sprites[emotionName];

            if (bodyLeftName != null)
                Appearance[1].Source = Sprites[bodyLeftName];
            else
                Appearance[1].Source = new BitmapImage();
            if (bodyLeftName != null)
                Appearance[2].Source = Sprites[bodyLeftName];
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
