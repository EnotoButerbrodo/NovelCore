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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainScene.Children.Add(BackgroundImage.Spot); 
            TextBox.TextAnimationComplete += delegate ()
            {
                IsSceneComplete = true;
            };
            
        }

        const string ResoursesPath = "../../../../Resourses/";
        const string CharactersZipPath = ResoursesPath + "Characters.zip";
        const string BackgroudsZipPath = ResoursesPath + "Backgrounds.zip";
        const string AudioZipPath = ResoursesPath + "Audio.zip";
        const string GuiZipPath = ResoursesPath + "gui.zip";
        Episode LoadedEpisode;
        Dictionary<string, Character> Characters = new Dictionary<string, Character>();
        Dictionary<string, BitmapImage> Backgrounds = new Dictionary<string, BitmapImage>();
        Dictionary<string, MemoryStream> Audio = new Dictionary<string, MemoryStream>();
        Dictionary<string, WaveOut> AudioPlayers = new Dictionary<string, WaveOut>();
        Storyboard SceneAnimation = new Storyboard();
        SceneImage BackgroundImage = new SceneImage();
        bool IsSceneComplete = false;
        static CancellationTokenSource Skip = new CancellationTokenSource();
        static CancellationToken SkipToken = Skip.Token;
        int SceneCounter = 0;
        
        void PlayScene(Scene scene)
        {
            IsSceneComplete = false;
            TextBox.ClearText();
            SetupBackgroud(scene.BackgroundConfig);
            SetupCharactersAppearance(scene.CharactersConfig);
            StartPlayAudio(scene.AudioConfig);
            
            SetupSceneAnimation(scene);
            SceneAnimation.Begin();
        }
        void SkipSceneAnimations()
        {
            SceneAnimation.SkipToFill();
        }
        void InitialClickSpot()
        {
            Grid ClickSpot = new Grid();
            ClickSpot.Background = Brushes.Transparent;
            Panel.SetZIndex(ClickSpot, 100);
            MainScene.Children.Add(ClickSpot);
            ClickSpot.Width = MainScene.ActualWidth;
            ClickSpot.Height = MainScene.ActualHeight;
            ClickSpot.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(ClickSpot_Click);

        }

        void ClickSpot_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsSceneComplete)
            {
                SkipSceneAnimations();
                return;
            }
            else
                PlayScene(LoadedEpisode[++SceneCounter % 2]);
        }
        

       

        #region Audio
            void StartPlayAudio(AudioArgs args)
        {
            if (!AudioPlayers.ContainsKey(args.Audio))
                AudioPlayers.Add(args.Audio, new WaveOut());
            else return;
            WaveFileReader reader = new WaveFileReader(Audio[args.Audio]);
            LoopStream wavSong = new LoopStream(reader);
            wavSong.EnableLooping = args.Loop;
            AudioPlayers[args.Audio] = new WaveOut();
            AudioPlayers[args.Audio].Init(wavSong);
            AudioPlayers[args.Audio].Play();  
        }
        void PlayAudio(string name)
        {
            AudioPlayers[name].Play();
        }
        void PauseAudio(string name)
        {
            AudioPlayers[name].Pause();
        }
        void StopAudio(string name)
        {
            AudioPlayers[name].Stop();
            AudioPlayers[name].Dispose();
        }
        #endregion

        async private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitialClickSpot();
            var textImage = ReadFromZip(GuiZipPath, "textbox.png").toBitmapImage();
            var nameImage = ReadFromZip(GuiZipPath, "namebox.png").toBitmapImage();
            TextBox.Setup(textImage, nameImage, MainGrid);
            LoadedEpisode = LoadEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json");
            LoadUsedResources(LoadedEpisode);
            BackgroundImage.Scale(1.2, 1.2);
            await Task.Delay(2000);
            PlayScene(LoadedEpisode[0]);
            //SceneAnimation.SkipToFill();
            //PlayScene(LoadedEpisode[1]);
            //SceneAnimation.SkipToFill();
            //DoubleAnimation anim = new DoubleAnimation();
            //anim.To = 1000;
            //anim.Duration = TimeSpan.FromSeconds(5);
            //anim.BeginTime = TimeSpan.FromMilliseconds(3000);
            //Storyboard.SetTarget(anim, Characters["Monika"].Spot);
            //Storyboard.SetTargetProperty(anim, new PropertyPath("(Canvas.Left)"));
            //SceneAnimation.Children.Add(anim);

            //await Task.Delay(3000);
            //SceneAnimation.Seek(TimeSpan.FromSeconds(2));

            //PlayScene(LoadedEpisode[0]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SkipSceneAnimations();
        }
    }
}
