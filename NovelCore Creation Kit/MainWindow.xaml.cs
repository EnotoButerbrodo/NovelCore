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
            scenes.Add(new Scene(
                SceneType.Text,
                new string[] { "Это первый фрейм" },
                new Dictionary<string, CharacterArgs>() { ["Monika"] = new CharacterArgs("Default.png")},
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
