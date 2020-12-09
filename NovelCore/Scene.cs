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
        public Scene(SceneType type, string[] text, CharacterArgs[] characterConfig,
            BackgroundArgs backConfig)
        {
            Type = type; Text = text;
            CharactersConfig = characterConfig; BackgroundConfig = backConfig;
        }
        [JsonPropertyName("fT")]
        public SceneType Type { get; set; }
        [JsonPropertyName("t")]
        public string[] Text { get; set; }
        [JsonPropertyName("cC")]
        public CharacterArgs[] CharactersConfig { get; set; }
        [JsonPropertyName("bC")]
        public BackgroundArgs BackgroundConfig { get; set; }
        [JsonPropertyName("aC")]
        public AudioArgs[] AudioConfig { get; set; }

    }
    [Serializable]
    public class CharacterArgs
    {
        public CharacterArgs() : base() { }
        public CharacterArgs(string character, string[] sprite, AnimationSettings animationConfig)
        {
            Character = character;
            Sprite = sprite;
            AnimationConfig = animationConfig;
        }
        [JsonPropertyName("c")]
        public string Character { get; set; }
        [JsonPropertyName("s")]
        public string[] Sprite { get; set; }
        [JsonPropertyName("anC")]
        public AnimationSettings AnimationConfig { get; set; }
        [JsonPropertyName("aS")]
        public string AnimationScriptName { get; set; }
    }

    [Serializable]
    public class BackgroundArgs
    {
        public BackgroundArgs() : base() { }
        public BackgroundArgs(string name, AnimationSettings animationConfig = null)
        {
            Background = name;
            AnimationConfig = animationConfig;
        }

        [JsonPropertyName("b")]
        public string Background { get; set; }
        [JsonPropertyName("BAA")]
        public AnimationSettings AnimationConfig { get; set; }
        [JsonPropertyName("bSrp")]
        public string AnimationScriptName { get; set; }
    }
    
    [Serializable]
    public class AudioArgs
    {
        public AudioArgs() : base () { }
        public AudioArgs(string audio, bool loop)
        {
            Audio = audio;
            Loop = loop;
        }
        [JsonPropertyName("a")]
        public string Audio { get; set; }
        [JsonPropertyName("lo")]
        public bool Loop { get; set; }
    }

    public enum SceneType
    {
        Empty,
        Text,
        DoubleText
    }
}
