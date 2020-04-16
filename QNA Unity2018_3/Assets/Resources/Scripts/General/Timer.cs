using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [System.Serializable]
    public class MinuteTimer
    {
        public bool Active = true;
        public bool AutomaticReset = true;
        public MinuteTime timer { get; protected set; } = new MinuteTime();
        public MinuteTime TargetTime = new MinuteTime(0, 1, 0);

        public void Reset() 
        {
            timer = TargetTime;
        }

        public void FixedUpdate()
        {
            if (!Active) return;

            float t = MinuteTime.ConvertToSeconds(timer);
            if (t <= 0 && AutomaticReset)
                Reset();
            else if (t > 0)
                timer -= Time.fixedDeltaTime;
        }
    }
}