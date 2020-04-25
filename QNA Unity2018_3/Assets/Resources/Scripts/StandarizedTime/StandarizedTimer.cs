using UnityEngine;

namespace AGAC.StandarizedTime
{
    public class StandarizedTimer : MonoBehaviour
    {
        #region UNITY METHODS
        private void FixedUpdate()
        {
            if (!Active) return;
            if (finished) return;

            if (timer <= 0)
                finished = true;
            else if (timer > 0)
                timer -= UnityEngine.Time.fixedDeltaTime;
        }
        #endregion

        #region VARIABLES
        public bool Active = true;
        public StandardTime TargetTime = new StandardTime(0, 1, 0);
        private float timer = 0f;
        private bool finished = false;
        #endregion

        #region PUBLIC METHODS
        public bool hasFinished() { return finished; }
        public StandardTime GetCurrentTime() { return StandardTime.FloatToSTime(timer); }
        public void Restart() 
        {
            Activate();
            finished = false;
            timer = StandardTime.STimeToFloat(GetTargetTime());
        }

        public void Activate() { Active = true; }
        public void Deactivate() { Active = false; }
        #endregion

        #region VIRTUAL METHODS
        protected virtual StandardTime GetTargetTime() 
        {
            return TargetTime;
        }
        #endregion
    }
}