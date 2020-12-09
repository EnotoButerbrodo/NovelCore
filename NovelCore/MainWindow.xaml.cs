﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        }
        const string ResoursesPath = "../../../../Resourses/";
        const string CharactersZipPath = ResoursesPath + "Characters.zip";
        const string BackgroudsZipPath = ResoursesPath + "Backgrounds.zip";
        const string AudioZipPath = ResoursesPath + "Audio.zip";
        Episode LoadedEpisode;
        Dictionary<string, Character> Characters = new Dictionary<string, Character>();
        Dictionary<string, BitmapImage> Backgrounds = new Dictionary<string, BitmapImage>();
        Dictionary<string, MemoryStream> Audio = new Dictionary<string, MemoryStream>();
        Dictionary<string, WaveOut> AudioPlayers = new Dictionary<string, WaveOut>();
        //WaveFileReader reader;
        //LoopStream wavSong;
        //WaveOut AudioPlayer;
        void PlayScene()
        {
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
        void LoadCharacters(string zipPath, Dictionary<string, string[]> sprites)
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
        void SetupBackgroud(BackgroundArgs args)
        {
            BackgroudImage.Source = Backgrounds[args.Background];
        }
        void SetupCharactersAppearance(Dictionary<string, CharacterArgs> args) 
        {
            foreach(var character in args)
            {
                Characters[character.Key].SetAppearance(character.Value.Sprite);
            }
        }
        void SetupCharactersAnimation(Dictionary<string, CharacterArgs> args)
        {
            foreach (var character in args)
            {
                BeginCharacterAnimation(Characters[character.Key],
                    character.Value.AnimationConfig);
            }
        }
        void BeginCharacterAnimation(Character character, AnimationSettings args)
        {

            if (args.StartPoint.X != args.EndPoint.X)
            {
                DoubleAnimation anim_X = new DoubleAnimation(args.EndPoint.X, new TimeSpan(0, 0, 0, 0, args.Speed));
                character.Spot.BeginAnimation(Canvas.LeftProperty, anim_X);
            }
            if (args.StartPoint.Y != args.EndPoint.Y)
            {
                DoubleAnimation anim_Y = new DoubleAnimation(args.EndPoint.Y, new TimeSpan(0, 0, 0, 0, args.Speed));
                character.Spot.BeginAnimation(Canvas.LeftProperty, anim_Y);
            }

        }
        void LoadAudio(string zipPath, string[] audios)
        {
            foreach(var audio in audios)
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
        void StartPlayAudio(string name, bool loop)
        {
            if(!AudioPlayers.ContainsKey(name))
                AudioPlayers.Add(name, new WaveOut());
            WaveFileReader reader = new WaveFileReader(Audio[name]);
            LoopStream wavSong = new LoopStream(reader);
            wavSong.EnableLooping = loop;
            AudioPlayers[name] = new WaveOut();
            AudioPlayers[name].Init(wavSong);
            AudioPlayers[name].Play();  
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
            AudioPlayers.Remove(name);
        }

        async private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var loadEpisode = LoadEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json");
            LoadCharacters(CharactersZipPath, loadEpisode.UsedSprites);
            LoadBackgrouds(BackgroudsZipPath, loadEpisode.UsedBackgrounds);

            SetupCharactersAppearance(loadEpisode[0].CharactersConfig);
            await Task.Delay(1000);
            SetupCharactersAnimation(loadEpisode[0].CharactersConfig);

            LoadAudio(AudioZipPath, new string[] { "TestSound.wav", "SilverfishDeath1.wav"});
            StartPlayAudio("TestSound.wav", true);
            StartPlayAudio("SilverfishDeath1.wav", true);
            await Task.Delay(1000);
            PauseAudio("SilverfishDeath1.wav");
            await Task.Delay(3000);
            PlayAudio("SilverfishDeath1.wav");


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

    }
}
