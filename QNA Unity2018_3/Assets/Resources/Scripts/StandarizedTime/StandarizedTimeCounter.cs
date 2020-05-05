
using UnityEngine;

namespace AGAC.StandarizedTime
{
    public class StandarizedTimeCounter : MonoBehaviour
    {
        #region UNITY METHODS
        private void FixedUpdate()
        {
            if (!Active) return;

            counter += Time.fixedDeltaTime;
        }
        #endregion

        #region VARIABLES
        public bool Active = true;

        private float counter = 0f;
        #endregion

        #region PUBLIC METHODS
        public StandardTime GetCurrentTime() { return StandardTime.FloatToSTime(counter); }
        public void Restart()
        {
            Activate();
            counter = 0f;
        }
        public void Activate() { Active = true; }
        public void Deactivate() { Active = false; }
        #endregion
    }
}
