using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Ionic.Zip;
using NAudio.Wave;
using Plugin.SimpleAudioPlayer;

namespace NovelCore
{

    public partial class MainWindow : Window
    {
        #region SceneSetup
        void SetupBackgroud(BackgroundArgs args)
        {
            BackgroundImage.SetImage(Backgrounds[args.Background]);
        }
        void SetupBackgroundAnimation(AnimationSettings args)
        {
            //SceneBackground.Move(args);
        }
        void SetupCharactersAppearance(CharacterArgs[] args)
        {
            foreach (var arg in args)
            {
                Characters[arg.Character].SetAppearance(arg.Sprite);

            }
        }
        void SetupSceneAnimation(Scene scene)
        {
            SceneAnimation = new Storyboard();

            SceneAnimation.Completed += new EventHandler((o, e) =>
            {
                TextBox.SetText(scene.Text[0]);
            });

            void AddToStroyboard(Grid Spot, AnimationSettings args)
            {
                if (args.EndPoint.X != null)
                {
                    DoubleAnimation anim_X = new DoubleAnimation((double)args.EndPoint.X, TimeSpan.FromMilliseconds(args.Speed));
                    Storyboard.SetTarget(anim_X, Spot);
                    Storyboard.SetTargetProperty(anim_X, new PropertyPath("(Canvas.Left)"));
                    SceneAnimation.Children.Add(anim_X);
                }
                if (args.EndPoint.Y != null)
                {
                    DoubleAnimation anim_Y = new DoubleAnimation((double)args.EndPoint.Y, TimeSpan.FromMilliseconds(args.Speed));
                    Storyboard.SetTarget(anim_Y, Spot);
                    Storyboard.SetTargetProperty(anim_Y, new PropertyPath("(Canvas.Bottom)"));
                    SceneAnimation.Children.Add(anim_Y);
                }
            }
            //Персонажи
            foreach (var charAnim in scene.CharactersConfig)
            {
                if (charAnim.AnimationConfig.Timing == AnimationTiming.AtBegin)
                    AddToStroyboard(Characters[charAnim.Character].Spot,
                    charAnim.AnimationConfig);
            }
            //Задний фон
        }


        #endregion
    }
}
