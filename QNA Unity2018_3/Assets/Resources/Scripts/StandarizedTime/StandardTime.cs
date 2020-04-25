using System.Numerics;
using UnityEngine;
using AGAC.StandarizedTime.Operations;

namespace AGAC.StandarizedTime
{
    public enum StandardTimeValues { Minutes, Seconds, Milliseconds, counter }

    [System.Serializable]
    public sealed class StandardTime
    {
        #region Constructors
        public StandardTime(int minutes = 0, int seconds = 0, int milliseconds = 0) :
            this(new int[3] { minutes, seconds, milliseconds })
        { }

        public StandardTime(int[] time_values)
        {
            if (time_values == null || time_values.Length <= 0)
                time_values = new int[1] { 0 };

            TimeValues = new int[TimeTypesCounter];
            for (int value = 0; value < time_values.Length; ++value)
            {
                if (value >= TimeTypesCounter) return;
                TimeValues[value] = time_values[value];
            }

            for (int value = time_values.Length; value < TimeTypesCounter; ++value)
                TimeValues[value] = 0;
        }
        #endregion

        #region Variables
        public int Minutes
        {
            get
            {
                return GetValue(StandardTimeValues.Minutes);
            }
            set
            {
                SetValue(StandardTimeValues.Minutes, value);
            }
        }
        public int Seconds
        {
            get
            {
                return GetValue(StandardTimeValues.Seconds);
            }
            set
            {
                SetValue(StandardTimeValues.Seconds, value);
            }
        }
        public int Milliseconds {
            get
            {
                return GetValue(StandardTimeValues.Milliseconds);
            }
            set
            {
                SetValue(StandardTimeValues.Milliseconds, value);
            }
        }

        public int[] TimeValues;

        public static string[] TimeValuesAbreviations 
        {
            get 
            {
                return new string[] { "Min", "Sec", "Millis" };
            }
        }

        private static int TimeTypesCounter { get { return (int)StandardTimeValues.counter; } }

        [SerializeField] private bool displayed = false;
        #endregion

        public static StandardTime zero { get { return new StandardTime(0, 0, 0); } }

        #region PUBLIC METHODS
        public static StandardTime FloatToSTime(float seconds)
        {
            return StandardTimeConverter.ToStandardTime(seconds);
        }
        public static float STimeToFloat(StandardTime time)
        {
            return StandardTimeConverter.ToFloat(time);
        }
        public override string ToString()
        {
            string str = string.Empty;
            for (int value = 0; value < TimeValues.Length; ++value)
                str += TimeValues[value].ToString() + " : ";
            str.TrimEnd(':');
            return str;
        }

        public int GetValue(StandardTimeValues type) 
        {
           if(!StandardTimeVerifyer.isAcceptableTimeType(type)) return 0;
            return TimeValues[(int)type];
        }
        public void SetValue(StandardTimeValues type, int value) 
        {
            if (StandardTimeVerifyer.isAcceptableTimeType(type)) return;
            TimeValues[(int)type] = value;
        }
        #endregion

        #region Operators
        public static StandardTime operator +(StandardTime a, StandardTime b)
        {
            for (int value = 0; value < a.TimeValues.Length; ++value)
                a.TimeValues[value] += b.TimeValues[value];

            a = GetVerifiedTime(a);
            return a;
        }
        public static StandardTime operator +(StandardTime time, float seconds) 
        {
            time += FloatToSTime(seconds);
            return time;
        }

        public static StandardTime operator -(StandardTime a, StandardTime b)
        {
            for (int value = 0; value < a.TimeValues.Length; ++value)
                a.TimeValues[value] -= b.TimeValues[value];

            a = GetVerifiedTime(a);
            return a;
        }
        public static StandardTime operator -(StandardTime time, float seconds)
        {
            time-= FloatToSTime(seconds);
            return time;
        }
        public static StandardTime operator *(int d, StandardTime time)
        {
            return time * d;
        }
        public static StandardTime operator *(StandardTime time, int d)
        {
            for (int value = 0; value < time.TimeValues.Length; ++value)
                time.TimeValues[value] *= d;

            time = GetVerifiedTime(time);
            return time;
        }
        public static StandardTime operator /(StandardTime time, int d)
        {
            if (d == 0) return StandardTime.zero;
            for (int value = 0; value < time.TimeValues.Length; ++value)
                time.TimeValues[value] /= d;


            time = GetVerifiedTime(time);
            return time;
        }
        #endregion

        #region PRIVATE METHODS
        private static StandardTime GetVerifiedTime(StandardTime time) 
        {
            return StandardTimeVerifyer.GetVerifiedTime(time);
        }
        #endregion
    }
}