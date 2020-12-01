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
            TestSave();
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

        void TestSave()
        {
            //Создать сцену
            //СОздать эпизод и сохранить его
            List<Scene> scenes = new List<Scene>();
            scenes.Add(new Scene(
                SceneType.Text,
                new string[] { "Это первый фрейм" },
                new Dictionary<string, ActorArgs>() { ["Monika"] = new ActorArgs("Default.png")},
                new BackgroundArgs("Class1.png")
                )
            );

            Episode NewEpisode = new Episode(
                "FirstEpisode",
                new string[] { "Class1.png" },
                new Dictionary<string, string[]> { ["Monika"] = new string[] { "Default.png" } },
                scenes.ToArray()
                );
            SaveEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json", NewEpisode);
        }
    }
}
