
namespace AGAC.StandarizedTime.Operations
{
    public static class StandardTimeConverter 
    {
        public static StandardTime ToStandardTime(float seconds)
        {
            int secondsInMinute = GetMaxValue(StandardTimeValues.Minutes);
            int millisInSecond = StandardTimeVerifyer.GetMaximumTime(StandardTimeValues.Milliseconds);

            StandardTime time = StandardTime.zero;
            time.Minutes = (int)(seconds / secondsInMinute);
            time.Seconds = (int)(seconds - (secondsInMinute * time.Minutes));
            time.Milliseconds = (int)((seconds - ((int)seconds)) * millisInSecond);
            return time;
        }
        public static float ToFloat(StandardTime time)
        {
            int secondsInMinute = GetMaxValue(StandardTimeValues.Minutes);
            int millisInSecond = StandardTimeVerifyer.GetMaximumTime(StandardTimeValues.Milliseconds);

            float resulting_seconds = 0f;
            resulting_seconds += time.Milliseconds / millisInSecond;
            resulting_seconds += time.Seconds;
            resulting_seconds += time.Minutes * secondsInMinute;
            return resulting_seconds;
        }

        private static int GetMaxValue(StandardTimeValues type)
        {
            return StandardTimeVerifyer.GetMaximumTime(type);
        }
    }
}