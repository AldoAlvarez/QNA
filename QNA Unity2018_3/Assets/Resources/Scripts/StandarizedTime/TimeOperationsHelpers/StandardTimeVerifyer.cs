
namespace AGAC.StandarizedTime.Operations
{
    public static class StandardTimeVerifyer
    {
        private static uint[] maximum_time_values;


        public static StandardTime GetVerifiedTime(StandardTime time)
        {
            int counter = (int)StandardTimeValues.counter;
            for (int value = counter - 1; value > 0; --value) 
                time = CheckForOvertime(time, value);

            return time;
        }

        private static StandardTime CheckForOvertime(StandardTime time, int timeValueIndex)
        {
            int max_value = StandardTimeVerifyer.GetMaximumTime((StandardTimeValues)timeValueIndex);
            OvertimeContainer overtimeContainer = VerifyTimeVariable(time.TimeValues[timeValueIndex], max_value);
            time.TimeValues[timeValueIndex] = overtimeContainer.ProperTime;
            if (timeValueIndex >= 1)
                time.TimeValues[timeValueIndex - 1] += overtimeContainer.Overtime;
            return time;
        }

        public static int GetMaximumTime(StandardTimeValues type)
        {
            if (!isAcceptableTimeType(type)) return 0;
            SetMaximumTimeValues();
            return (int)maximum_time_values[(int)type];
        }

        public static void SetMaximumTimeValues()
        {
            int TimeTypesCounter = (int)StandardTimeValues.counter;
            if (maximum_time_values != null && maximum_time_values.Length == TimeTypesCounter) return;

            maximum_time_values = new uint[TimeTypesCounter];
            maximum_time_values[(int)StandardTimeValues.Minutes] = 60;
            maximum_time_values[(int)StandardTimeValues.Seconds] = 60;
            maximum_time_values[(int)StandardTimeValues.Milliseconds] = 1000;
        }

        public static bool isAcceptableTimeType(StandardTimeValues type)
        {
            int type_value = (int)type;
            if (type_value < 0 || type_value >= (int)StandardTimeValues.counter)
                return false;
            return true;
        }

        #region PRIVATE METHODS
        private static OvertimeContainer VerifyTimeVariable(int time_variable, int max_time_value)
        {
            OvertimeContainer Result = new OvertimeContainer(time_variable, 0);

            if (max_time_value == 0) return Result;
            if (max_time_value < 0) max_time_value *= -1;

            if (time_variable >= max_time_value)
            {
                Result.Overtime = time_variable / max_time_value;
                Result.ProperTime = time_variable - (max_time_value * Result.Overtime);
            }
            else if (time_variable < 0)
            {
                Result.Overtime = (time_variable / max_time_value) - 1;
                Result.ProperTime = time_variable + (max_time_value * -Result.Overtime);
            }

            return Result;
        }
        #endregion
    }
}