using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace Tracker.Resources
{
    public class DisplayRole : MonoBehaviour
    {

        private InputAction _inputAction;
        private TrackedPoseDriver _trackedPoseDriver;
        [SerializeField] private TMP_Text textMeshPro;

        void Start()
        {
            var parent = transform.parent;
            _trackedPoseDriver = parent.GetComponent<TrackedPoseDriver>();
            textMeshPro.text = parent.name + "\n" + _trackedPoseDriver.positionInput.reference.name + "\n" + _trackedPoseDriver.rotationInput.reference.name + "\n" + _trackedPoseDriver.trackingStateInput.reference.name;
        }
    }
}

