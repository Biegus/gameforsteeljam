using UnityEngine;

namespace Game
{
    public class Timer
    {
        public float StartTime;
        private float TimeToElapse;
        public Timer(float time)
        {
            StartTime = Time.time;
            TimeToElapse = time;
        }

        public bool Done => Time.time - StartTime >= TimeToElapse;

        public void Reset()
        {
            StartTime = Time.time;
        }
        public bool Push()
        {
            if (Done)
            {
               Reset();
               return true;
            }

            return false;
        }
    }
}