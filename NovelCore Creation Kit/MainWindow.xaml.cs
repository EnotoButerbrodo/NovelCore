using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NovelCore;
using Ionic.Zip;
using System.IO;

namespace NovelCore_Creation_Kit

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

        void SaveEpisode(string path, Episode episode)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                string ser_episode = JsonSerializer.Serialize<Episode>(episode, new JsonSerializerOptions { IgnoreNullValues = true });
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(ser_episode);
                }
            }
        }

        void CreateZip(string path, string name)
        {
            using(ZipFile zip = new ZipFile())
            {
                zip.Save($"{path}/{name}");
            }
        }
        void AddToZip(string zipPath, string filePath)
        {
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                //zip.AddDirectory(zipPath, "");
                zip.AddFile(filePath, "Monika");
                zip.AddFile(filePath, "Yuri");
                zip.Save(zipPath);
                MessageBox.Show(zip.Entries.Count.ToString());
            }
        }

        void TestSave()
        {
            //Создать сцену
            //СОздать эпизод и сохранить его
            List<Scene> scenes = new List<Scene>();
            //Scene 1
            SceneType st = SceneType.Text;
            string[] text = new string[] { "Приветик." };
            AnimationSettings settings = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
                new DoublePoint(-300.0, 0.0, 0.0), AnimationTiming.AtBegin, 1000);
            AnimationSettings settings2 = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
                new DoublePoint(300.0, 0.0, 0.0), AnimationTiming.AtBegin, 2000);
            var settings3 = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
               new DoublePoint(600.0, 0.0, 0.0), AnimationTiming.AtBegin, 3000);
            var chars = new CharacterArgs[]
            {
                new CharacterArgs("Monika", new string[] { "Default.png", null, null },
                settings),
                new CharacterArgs("lilly", new string[] { "lilly_basic_concerned_cas.png", null, null },
                settings2),
                new CharacterArgs("PMonika", new string[] { "j.png", "1l.png", "1r.png" },
                settings3)
            };
            var backConfig = new BackgroundArgs("Class1.png");
            scenes.Add(new Scene(st, text, chars, backConfig));

            //Scene 2
            st = SceneType.Text;
            text = new string[] { "Это я" };
            settings = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
                new DoublePoint(300.0, 0.0, 0.0), AnimationTiming.AtBegin, 1000);
            settings2 = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
                new DoublePoint(-300.0, 0.0, 0.0), AnimationTiming.AtBegin, 1000);
            settings3 = new AnimationSettings(new DoublePoint(0.0, 0.0, 0.0),
               new DoublePoint(-600.0, 0.0, 0.0), AnimationTiming.AtBegin, 1000);
            chars = new CharacterArgs[]
            {
                new CharacterArgs("Monika", new string[] { "Default_confusion.png", null, null },
                settings),
                new CharacterArgs("lilly", new string[] { "lilly_back_sad_cas.png", null, null },
                settings2),
                new CharacterArgs("PMonika", new string[] { "m.png", "1l.png", "1r.png" },
                settings3)
            };
            backConfig = new BackgroundArgs("Class1.png");
            scenes.Add(new Scene(st, text, chars, backConfig));


            //Finall Episode config
            string episode_name = "First Episode";
            Dictionary<string, string[]> usedSprites = new Dictionary<string, string[]>
            {
                ["Monika"] = new string[] { "Default.png", "Default_confusion.png", "Flirty_angry.png" },
                ["lilly"] = new string[] { "lilly_back_devious.png", "lilly_back_sad_cas.png", "lilly_back_smile_cas.png", "lilly_basic_concerned_cas.png" },
                ["PMonika"] = new string[] {"j.png", "1l.png", "1r.png", "m.png" }
            };
            var usedBackgrounds = new string[] { "Class1.png", "Class2.png" };

            var episodeToSave = new Episode(episode_name, usedBackgrounds,
                usedSprites, scenes.ToArray());



            SaveEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json", episodeToSave);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TestSave();
        }

        private void SaveZip_click(object sender, RoutedEventArgs e)
        {
            CreateZip(@"S:\Users\Игорь\source\repos\NovelCore", "Hi2.zip");
        }

        private void AddtoZip_click(object sender, RoutedEventArgs e)
        {
            AddToZip(@"S:\Users\Игорь\source\repos\NovelCore\Hi22.zip", "АНИМЕ.jpg");
        }
    }
}
