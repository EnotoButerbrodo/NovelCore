using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace NovelCore
{
    [Serializable]
    public class Episode
    {
        public Episode() : base() { }
        public Episode(
            string name,
            string[] usedBackgrouds,
            Dictionary<string, string[]> usedSprites,
            Scene[] scenes
            )
        {
            Name = name;
            UsedBackgrounds = usedBackgrouds;
            UsedSprites = usedSprites;
            Scenes = scenes;
        }
        public string Name { get; set; } //Имя сцены
        public string[] UsedBackgrounds { get; set; } //Набор используемых фонов
        public Dictionary<string, string[]> UsedSprites { get; set; } //<Имя персонажа, набор спрайтов
        public Scene[] Scenes { get; set; }
        public Scene this[int number]
        {
            get
            {
                return Scenes[number];
            }
        }
        public int Lenght => Scenes.Length;

    }
}
