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
        const string ResoursesPath = "../../../../Resourses/";
        const string CharactersZipPath = ResoursesPath + "Characters.zip";
        const string BackgroudsZipPath = ResoursesPath + "Backgrounds.zip";
        Episode LoadedEpisode;
        Dictionary<string, Character> Characters = new Dictionary<string, Character>();
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
        void SetupCharacterAppearance(Dictionary<string, CharacterArgs> args) 
        {
            foreach(var character in args)
            {
                Characters[character.Key].SetAppearance(character.Value.Sprite);
            }
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
            LoadCharacters(CharactersZipPath, test);
            LoadBackgrouds(BackgroudsZipPath, new string[] { "Class1.png" });
            SetupCharacterAppearance(new Dictionary<string, CharacterArgs>
            {
                ["Monika"] = new CharacterArgs(new string[] { "Default.png",null, null })
            });
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

        //public Dictionary<string, List<MemoryStream>> ReadFromZip(string zipPath, Dictionary<string, string[]> spriteNames)
        //{
        //    Dictionary<string, List<MemoryStream>> buff = new Dictionary<string, List<MemoryStream>>();

        //    using (ZipFile zip = ZipFile.Read(zipPath))
        //    {
        //        foreach (var character in spriteNames)
        //        {
        //            buff.Add(character.Key, new List<MemoryStream>());
        //            foreach (string sprite in character.Value)
        //            {
        //                MemoryStream stream = new MemoryStream();
        //                zip[$"{character.Key}\\{sprite}"].Extract(stream);
        //                buff[character.Key].Add(stream);
        //            }
        //        }
        //    }
        //    return buff;
        //}

    }
}
