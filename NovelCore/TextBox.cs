using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Threading;

namespace NovelCore
{

    static class TextBox
    {
        public static void Setup(BitmapImage textboxImage, BitmapImage nameboxImage,
            Grid mainScene)
        {
            //Spot
            Spot.VerticalAlignment = VerticalAlignment.Stretch;
            Spot.HorizontalAlignment = HorizontalAlignment.Stretch;
            Spot.SetValue(Grid.RowProperty, 4);
            Spot.SetValue(Grid.ColumnProperty, 2);
            Spot.SetValue(Grid.ColumnSpanProperty, 4);
            mainScene.Children.Add(Spot);

            //Text
            //--Image--
            TextboxImage.Source = textboxImage;
            TextboxImage.Stretch = Stretch.Fill;
            Spot.Children.Add(TextboxImage);
            //--Spot--
            TextSpot.TextWrapping = TextWrapping.Wrap;
            TextSpot.Text = "";
            TextSpot.Foreground = Brushes.White;
            TextSpot.VerticalAlignment = VerticalAlignment.Stretch;
            TextSpot.HorizontalAlignment = HorizontalAlignment.Stretch;
            TextSpot.Margin = new Thickness(20, 20, 20, 20);
            TextSpot.FontSize = 28;
            Spot.Children.Add(TextSpot);

            //NameboxImage
            NameboxImage.Source = nameboxImage;
            NameboxSpot.Children.Add(NameboxImage);
            NameboxSpot.Children.Add(NameboxTextSpot);
            NameboxTextSpot.Text = "Monika";

        }
        static string Speaker { get; set; }
        static SceneType Type { get; set; }
        static bool Visible { get; set; }
        static Grid Spot { get; set; } = new Grid();
        static Grid NameboxSpot { get; set; } = new Grid();
        static Image TextboxImage { get; set; } = new Image();
        static Image NameboxImage { get; set; } = new Image();
        static TextBlock TextSpot { get; set; } = new TextBlock();
        static TextBlock NameboxTextSpot { get; set; } = new TextBlock();

        public static event Action TextAnimationComplete;
        public static void SetText(string text)
        {
            TextSpot.Text = text;
            TextAnimationComplete?.Invoke();
        }
        public static void ClearText()
        {
            TextSpot.Text = "";
        }

        async public static Task SetText(string text, int time, CancellationToken token)
        {
            TextSpot.Text = "";
            foreach (char s in text)
            {
                if (token.IsCancellationRequested)
                {
                    TextSpot.Text = text;
                    break;
                }
                TextSpot.Text += s;
                if (s == ' ') continue;
                await Task.Delay(time);
            }
            TextAnimationComplete?.Invoke();
        }
        public static void SetSpeaker(string name)
        {
            Speaker = name;
        }
    }
}

