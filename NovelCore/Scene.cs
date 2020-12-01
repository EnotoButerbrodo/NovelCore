using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NovelCore
{
    public class Scene
    {
        SceneType Type { get; }
        public string Background { get; }
        public Dictionary<string, ActorInfo> ActorsConfig { get;  }
        public BackgroundInfo BackgroundConfig { get; }
    }
    public class ActorInfo
    {
        public string Sprite { get;}
        public AnimationEventArgs ActorAnimationArgs { get; }
        public string AnimationScriptName { get; }

    }

    public struct BackgroundInfo
    {
        public string Background;
        public AnimationEventArgs BackAnimationArgs { get; }
    }
    enum SceneType
    {
        Empty,
        Text,
        DoubleText
    }
}
