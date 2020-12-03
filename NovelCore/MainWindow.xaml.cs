using System;
using System.Collections.Generic;
using System.IO;
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
using Ionic.Zip;

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
        Episode LoadedEpisode;
        Dictionary<string, Actor> Actors = new Dictionary<string, Actor>();
        Dictionary<string, BitmapImage> Backgrounds = new Dictionary<string, BitmapImage>();
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

        void SetupBackgrouds(string zipPath, string[] backgrouds)
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
        public MemoryStream ReadFromZip(string zipPath, string fileName)
        {
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                foreach (ZipEntry zipEntry in zip)
                {
                    if (zipEntry.FileName.Contains(fileName))
                    {
                        MemoryStream stream = new MemoryStream();
                        zipEntry.Extract(stream);
                        return stream;
                    }
                }
            }
            throw new Exception("Файл не найден");
        }

        public Dictionary<string, List<MemoryStream>> ReadFromZip(string zipPath, Dictionary<string, string[]> spriteNames)
        {
            Dictionary<string, List<MemoryStream>> buff = new Dictionary<string, List<MemoryStream>>();
            
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                foreach (var character in spriteNames)
                {
                    buff.Add(character.Key, new List<MemoryStream>());
                    foreach (string sprite in character.Value)
                    {
                        MemoryStream stream = new MemoryStream();
                        zip[$"Characters\\{character.Key}\\{sprite}"].Extract(stream);
                        buff[character.Key].Add(stream);
                    }
                }
            }
            return buff;
        }
        void TestLoad()
        {
            LoadedEpisode = LoadEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string[]> test = new Dictionary<string, string[]>
            {
                ["Monika"] = new string[] { "Default.png", "Default_confusion.png", "Flirty_angry.png" },
                ["lilly"] = new string[] { "lilly_back_devious.png", "lilly_back_sad_cas.png", "lilly_back_smile_cas.png", "lilly_basic_concerned_cas.png" }
            };
            var streams = ReadFromZip(@"S:\Users\Игорь\source\repos\NovelCore\images.zip",
                test);
        }

        
    }
}
