using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NovelCore
{
    class Scene
    {
        SceneType Type { get; set; }
        public string Background { get; set; }
        public ActorInfo[] Actors { get; set; }
    }
    public struct ActorInfo
    {
        public ActorInfo(string name, string[] sprites, Point position, string speech,
            ActorAction action)
        {
            Name = name;
            Sprites = sprites;
            Position = position;
            Speech = speech;
            Action = action;
        }
        public string Name { get; set; }
        public string[] Sprites { get; set; }
        public Point? Position { get; set; }
        public string Speech { get; set; }
        public ActorAction? Action { get; set; }

        public enum ActorAction
        {
            Enter,
            Leave
        }
    }

    public struct BackgroundInfo
    {
        public string Background { get; set; }

    }
    enum SceneType
    {
        Empty,
        Text,
        DoubleText
    }
}
