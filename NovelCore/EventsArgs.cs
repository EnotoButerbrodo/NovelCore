using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCore
{
    [Serializable]
    public class AnimationEventArgs
    {
        public Point point { get; }
        public Timing timing { get; }
        public double? Speed { get; }
        public string ScriptName { get; }
    }

    public enum Timing
    {
        AtBegin,
        OnTheEnd
    }
}
