using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace NovelCore
{
    [Serializable]
    public class Scene
    {
        public Scene(SceneType type, string[] text, Dictionary<string, ActorArgs> actorConfig,
            BackgroundArgs backConfig)
        {
            Type = type; Text = text;
            ActorsConfig = actorConfig; BackgroundConfig = backConfig;
        }
        public SceneType Type { get; set; }
        public string[] Text { get; set; }
        public Dictionary<string, ActorArgs> ActorsConfig { get; set; }
        public BackgroundArgs BackgroundConfig { get; set; }

    }
    [Serializable]
    public class ActorArgs
    {
        public ActorArgs(string sprite, AnimationEventArgs args = null)
        {
            Sprite = Sprite;
        }
        public string Sprite { get; set; }
        public AnimationEventArgs ActorAnimationArgs { get; set; }
        public string AnimationScriptName { get; set; }
    }

    [Serializable]
    public class BackgroundArgs
    {
        public BackgroundArgs(string name)
        {
            Background = name;
        }
        public string Background { get; set; }
        public AnimationEventArgs BackAnimationArgs { get; set; }
        public string BackScriptName { get; set; }
    }

    public enum SceneType
    {
        Empty,
        Text,
        DoubleText
    }
}
