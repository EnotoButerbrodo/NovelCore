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
            InitialClickSpot();
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

            
            //TextBox.SetText(scene.Text[0], 10);
            //await Task.Delay(2000)
            //Запретить переключение сцены
            //Задать или сменить задний фон
            //Задать если нужно декорации
            //Задать и применить если нужно анимации для сцены
            //Расставить персонажей на сцену
            //Задать им нужные спрайты
            //Применить к ним AtFirst анимации
            //Применить анимации отображения текста
            //Применить OnTheEnd анимации
            //Разрешить переключение сцены
        }
        void SkipSceneAnimations()
        {
            SceneAnimation.SkipToFill();
        }
        void InitialClickSpot()
        {
            Grid ClickSpot = new Grid();
            ClickSpot.Background = Brushes.Red;
            Panel.SetZIndex(ClickSpot, 100);
            MainScene.Children.Add(ClickSpot);
            ClickSpot.Width = 100;
            ClickSpot.Height = 100;
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
        #region Load
        Episode LoadEpisode(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                using (var reader = new StreamReader(fs))
                {
                    string file = reader.ReadToEnd();
                    return JsonSerializer.Deserialize<Episode>(file, new JsonSerializerOptions { IgnoreNullValues = true });
                }
            }
        }
        void LoadBackgrouds(string zipPath, string[] backgrouds)
        {
            foreach(string back in backgrouds)
            {
                if (!Backgrounds.ContainsKey(back))
                {
                    BitmapImage image = ReadFromZip(zipPath, back).toBitmapImage();
                    Backgrounds.Add(back, image);
                }
                else continue;
            }
        }
        void LoadSprites(string zipPath, Dictionary<string, string[]> sprites)
        {
            //Пройтись по всем ключам словаря. Каждый ключ - какой то герой
            //Если в словаре существующих героев нет такого, создаем нового
            //Пройтись по всем спрайтам прочитанного списка. Если персонаж не имеет
            //...нужных спрайтов - загрузить их и добавить в коллекцию герою
            foreach(var character in sprites)
            {
                if (!Characters.ContainsKey(character.Key))
                {
                    Characters.Add(character.Key, new Character(character.Key));
                    Characters[character.Key].EnterTheScene(MainScene);
                }
                foreach(var sprite in character.Value)
                {
                    if (!Characters[character.Key].SpriteInCollection(sprite))
                    {
                        BitmapImage image = ReadFromZip(zipPath,
                            $"{character.Key}\\{sprite}").toBitmapImage();
                        Characters[character.Key].AddSprite(sprite, image);
                    }
                }            
            }
        }
        void LoadAudio(string zipPath, string[] audios)
        {
            foreach (var audio in audios)
            {
                if (!Audio.ContainsKey(audio))
                {
                    var buff = ReadFromZip(zipPath, audio);
                    buff.Position = 0;
                    Audio.Add(audio, buff);
                }
                else continue;
            }
        }
        void LoadUsedResources(Episode episode)
        {
            LoadBackgrouds(BackgroudsZipPath, episode.UsedBackgrounds);
            LoadSprites(CharactersZipPath, episode.UsedSprites);
            LoadAudio(AudioZipPath, episode.UsedAudio);
        }

        public MemoryStream ReadFromZip(string zipPath, string fileName)
        {
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                MemoryStream stream = new MemoryStream();
                zip[fileName].Extract(stream);
                return stream;
            }
            throw new Exception("Файл не найден");
        }
        #endregion

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
            foreach(var arg in args)
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
            var textImage = ReadFromZip(GuiZipPath, "textbox.png").toBitmapImage();
            var nameImage = ReadFromZip(GuiZipPath, "namebox.png").toBitmapImage();
            TextBox.Setup(textImage, nameImage, MainGrid);
            LoadedEpisode = LoadEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json");
            LoadUsedResources(LoadedEpisode);
            BackgroundImage.Scale(1.2, 1.2);
            await Task.Delay(2000);
            PlayScene(LoadedEpisode[0]);
            SceneAnimation.Begin();
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
