namespace General
{
    [System.Serializable]
    public struct MinuteTime
    {
        #region Constructors
        public MinuteTime(int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }
        #endregion

        #region Variables
        public int Milliseconds;
        public int Seconds;
        public int Minutes;
        #endregion

        public static MinuteTime zero { get { return new MinuteTime(0, 0, 0); } }

        public static MinuteTime ConvertToMT(float seconds)
        {
            MinuteTime MT = zero;
            MT.Minutes = ((int)seconds) / 60;
            MT.Seconds = (int)seconds - (60 * MT.Minutes);
            MT.Milliseconds = (int)((seconds - ((int)seconds)) * 1000);
            return MT;
        }
        public static float ConvertToSeconds(MinuteTime MT)
        {
            float resulting_seconds = 0f;
            resulting_seconds += MT.Milliseconds / 1000;
            resulting_seconds += MT.Seconds;
            resulting_seconds += MT.Minutes * 60;
            return resulting_seconds;
        }
        public override string ToString()
        {
            string str = Minutes.ToString() + ": " + Seconds.ToString();
            return str;
        }
        #region Operators
        public static MinuteTime operator +(MinuteTime a, MinuteTime b)
        {
            a.Milliseconds += b.Milliseconds;
            a.Seconds += b.Seconds;
            a.Minutes += b.Minutes;

            a = VerifyTime(a);
            return a;
        }
        public static MinuteTime operator +(MinuteTime a, float seconds) 
        {
            a += ConvertToMT(seconds);
            return a;
        }

        public static MinuteTime operator -(MinuteTime a, MinuteTime b)
        {
            a.Milliseconds -= b.Milliseconds;
            a.Seconds -= b.Seconds;
            a.Minutes -= b.Minutes;

            a = VerifyTime(a);
            return a;
        }
        public static MinuteTime operator -(MinuteTime a, float seconds)
        {
            a -= ConvertToMT(seconds);
            return a;
        }
        public static MinuteTime operator *(int d, MinuteTime a)
        {
            a.Milliseconds *= d;
            a.Seconds *= d;
            a.Minutes *= d;

            a = VerifyTime(a);
            return a;
        }
        public static MinuteTime operator *(MinuteTime a, int d)
        {
            a.Milliseconds *= d;
            a.Seconds *= d;
            a.Minutes *= d;

            a = VerifyTime(a);
            return a;
        }
        public static MinuteTime operator /(MinuteTime a, int d)
        {
            if (d == 0) return MinuteTime.zero;
            a.Milliseconds /= d;
            a.Seconds /= d;
            a.Minutes /= d;

            a = VerifyTime(a);
            return a;
        }
        #endregion

        private struct TimeOperations
        {
            public TimeOperations(int resulting_time, int time_difference)
            {
                ResultinMTime = resulting_time;
                TimeDifference = time_difference;
            }
            public int ResultinMTime;
            public int TimeDifference;
        }

        #region Private operator methods
        private static MinuteTime VerifyTime(MinuteTime MT)
        {
            TimeOperations to_milliseconds = VerifyTimeVariable(MT.Milliseconds, 1000);
            MT.Milliseconds = to_milliseconds.ResultinMTime;
            MT.Seconds += to_milliseconds.TimeDifference;

            TimeOperations to_seconds = VerifyTimeVariable(MT.Seconds, 60);
            MT.Seconds = to_seconds.ResultinMTime;
            MT.Minutes += to_seconds.TimeDifference;

            return MT;
        }

        private static TimeOperations VerifyTimeVariable(int time_variable, int time_comparison)
        {
            TimeOperations Result = new TimeOperations(time_variable, 0);

            if (time_comparison == 0) return Result;
            if (time_comparison < 0) time_comparison *= -1;

            if (time_variable >= time_comparison)
            {
                Result.TimeDifference = time_variable / time_comparison;
                Result.ResultinMTime = time_variable - (time_comparison * Result.TimeDifference);
            }
            else if (time_variable < 0)
            {
                Result.TimeDifference = (time_variable / time_comparison) - 1;
                Result.ResultinMTime = time_variable + (time_comparison * -Result.TimeDifference);
            }

            return Result;
        }
        #endregion
    }
}