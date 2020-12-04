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
        public Scene() : base() { }
        public Scene(SceneType type, string[] text, Dictionary<string, CharacterArgs> actorConfig,
            BackgroundArgs backConfig)
        {
            Type = type; Text = text;
            ActorsConfig = actorConfig; BackgroundConfig = backConfig;
        }
        [JsonPropertyName("fT")]
        public SceneType Type { get; set; }
        [JsonPropertyName("t")]
        public string[] Text { get; set; }
        [JsonPropertyName("cC")]
        public Dictionary<string, CharacterArgs> ActorsConfig { get; set; }
        [JsonPropertyName("bC")]
        public BackgroundArgs BackgroundConfig { get; set; }

    }
    [Serializable]
    public class CharacterArgs
    {
        public CharacterArgs() : base() { }
        public CharacterArgs(string[] sprite, AnimationEventArgs args = null)
        {
            Sprite = sprite;
        }

        [JsonPropertyName("s")]
        public string[] Sprite { get; set; }
        [JsonPropertyName("A3")]
        public AnimationEventArgs ActorAnimationArgs { get; set; }
        [JsonPropertyName("aS")]
        public string AnimationScriptName { get; set; }
    }

    [Serializable]
    public class BackgroundArgs
    {
        public BackgroundArgs() : base() { }
        public BackgroundArgs(string name)
        {
            Background = name;
        }

        [JsonPropertyName("b")]
        public string Background { get; set; }
        [JsonPropertyName("BAA")]
        public AnimationEventArgs BackAnimationArgs { get; set; }
        [JsonPropertyName("bSrp")]
        public string BackScriptName { get; set; }
    }

    public enum SceneType
    {
        Empty,
        Text,
        DoubleText
    }
}
