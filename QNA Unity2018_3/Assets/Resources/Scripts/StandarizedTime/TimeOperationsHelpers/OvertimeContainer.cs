
namespace AGAC.StandarizedTime.Operations
{
    public struct OvertimeContainer
    {
        public OvertimeContainer(int proper_time, int overtime)
        {
            ProperTime = proper_time;
            Overtime = overtime;
        }
        public int ProperTime;
        public int Overtime;
    }
}