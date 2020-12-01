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
