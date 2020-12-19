using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NovelCore
{
    [Serializable]
    public class AnimationSettings
    {
        public AnimationSettings() : base () { }
        public AnimationSettings(DoublePoint startPoint, DoublePoint endPoint,
            AnimationTiming timing, int speed, string scriptName = null)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Timing = timing;
            Speed = speed;
            ScriptName = scriptName;
        }
        [JsonPropertyName("sP")]
        public DoublePoint StartPoint { get; set; }
        [JsonPropertyName("eP")]
        public DoublePoint EndPoint { get; set; }
        [JsonPropertyName("ti")]
        public AnimationTiming Timing { get; set; }
        [JsonPropertyName("sp")]
        public int Speed { get; set; }
        public string ScriptName { get; set; }
    }
    [Serializable]
    public class DoublePoint
    {
        public DoublePoint() : base() { }
        public DoublePoint(double x, double y, double z) 
        {
            X = x; Y = y; Z = z;
        }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }

    }
    public enum AnimationTiming
    {
        AtBegin,
        OnTheEnd
    }
}
