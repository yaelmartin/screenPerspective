using UnityEngine;

namespace Tracker.Demos.Resources
{
    public class FPSlimiter : MonoBehaviour

    {
        public int FPS = 30;
        void Start()
        {
            Application.targetFrameRate = FPS;
        }
    }
}