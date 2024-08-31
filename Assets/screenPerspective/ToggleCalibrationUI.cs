using PlasticGui.Help;
using UnityEngine;
using UnityEngine.Serialization;
using Tracker;

namespace screenPerspective
{
    public class ToggleCalibrationUI : MonoBehaviour
    {
        
        [SerializeField] [Tooltip("Keypress to toggle UI calibration")]
        private KeyCode keypress = KeyCode.R;
        
        private bool show = true;

        [SerializeField] private GameObject calibrationUI;
        [SerializeField] private TrackerOperations trackerOperations;
        private GameObject spawn;
        
        void Start()
        {
            spawn = trackerOperations.Spawn.gameObject;
            UiShow();
        }

        private void UiShow()
        {
            calibrationUI.SetActive(show);
            spawn.SetActive(show);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(keypress))
            {
                show = !show;
                UiShow();
            }
        }
    }
}
