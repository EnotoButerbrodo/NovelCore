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
        #region Load
        
        Episode LoadEpisode(string path)
        {//Дессериализация эпизода для его дальнейшего воспроизведения
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
        {//Принимает названия картинок и загружает их из игровых архивов
            foreach (string back in backgrouds)
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
        {//Загрузка по названию из архивов картинок персонажей
            foreach (var character in sprites)
            {
                if (!Characters.ContainsKey(character.Key))
                {
                    Characters.Add(character.Key, new Character(character.Key));
                    Characters[character.Key].EnterTheScene(MainScene);
                }
                foreach (var sprite in character.Value)
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
        {//Загрузка из архивов аудиофайлов
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
        {//Чтение любого файла из архива в виде MemoryStream
            using (ZipFile zip = ZipFile.Read(zipPath))
            {
                MemoryStream stream = new MemoryStream();
                zip[fileName].Extract(stream);
                return stream;
            }
            throw new Exception("Файл не найден");
        }
        #endregion
    }
}
