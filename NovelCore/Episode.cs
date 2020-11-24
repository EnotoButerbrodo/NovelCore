using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCore
{
    class Episode
    {
        public string Name { get; set; } //Имя сцены
        public string[] UsedBackgrounds { get; set; } //Набор используемых фонов
        public Dictionary<string, string[]> UsedSprites { get; set; } //<Имя персонажа, набор спрайтов
        public Scene[] Scenes { get; set; }
        
    }
}
