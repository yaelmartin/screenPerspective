using System.Collections;
using UnityEngine;

namespace Tracker.Demos.Resources.MultiTrackersScene
{
    // Due to having both the VR camera and the display camera set on Display 1,
    // We need to delay its activation
    public class CameraDisplay : MonoBehaviour
    {
        [SerializeField] private Camera cameraComponent;
        private void Awake()
        {
            cameraComponent.enabled = false;
        }
        void Start()
        {
            StartCoroutine(EnableCameraDelayed());
        }
        
        IEnumerator EnableCameraDelayed()
        {

            yield return new WaitForSeconds(2);
            cameraComponent.enabled = true;
        }
    }
}
