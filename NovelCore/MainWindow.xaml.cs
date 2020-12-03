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

        public List<MemoryStream> ReadFromZip(string zipPath, Dictionary<string, string[]> spriteNames)
        {
            List<MemoryStream> streams = new List<MemoryStream>();
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                foreach (var character in spriteNames)
                {
                    foreach (string sprite in character.Value)
                    {
                        streams.Add(new MemoryStream());
                        zip[$"Characters\\{character.Key}\\{sprite}"].Extract(streams[streams.Count - 1]);
                    }
                }
            }
            return streams;
        }
        void TestLoad()
        {
            LoadedEpisode = LoadEpisode(@"S:\Users\Игорь\source\repos\NovelCore\test.json");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var streams = ReadFromZip(@"S:\Users\Игорь\source\repos\NovelCore\images.zip",
                new Dictionary<string, string[]>
                {
                    ["Monika"] = new string[] { "Default.png" }
                });

            List<BitmapImage> images = new List<BitmapImage>();
            foreach(var str in streams)
            {
                images.Add(str.toBitmapImage());
            }
            TestImage.Source = images[0];
        }
    }
}
