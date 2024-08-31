using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;

namespace Tracker
{
    /// <summary>
    /// Used to align the room-scale area, and calibrate the offset between the tracker and real object
    /// </summary>
    public class TrackerOperations : TrackerConfigLoader
    {
        [SerializeField] private Transform targetedTracker;
        [SerializeField] private Transform originRoomScale;
        [SerializeField] private Transform spawn;
        private Transform _movableArea;
        private TrackedPoseDriver _trackerTrackedPoseDriver;
        
        private Vector3 _initialIrlObjectWithTrackerPosition;
        private Quaternion _initialIrlObjectWithTrackerRotation;
        
        public void RecenterTrackerToSpawn()
        {
            Vector3 offset = spawn.transform.position;
            offset -= (Tracker.position - originRoomScale.position);
            originRoomScale.position = offset;

            TrackerParameters.TrackerOriginOffset = originRoomScale.localPosition;

            if (irlObjectWithTracker != null)
            {
                UnParentIrlObjectAndReparent();  
            }
        }

        // Must be called when the Tracker is recentered first and hasn't moved since
        public void UnParentIrlObjectAndReparent()
        {
            irlObjectWithTracker.SetParent(_movableArea);
            irlObjectWithTracker.localPosition = _initialIrlObjectWithTrackerPosition;
            irlObjectWithTracker.localRotation = _initialIrlObjectWithTrackerRotation;
            irlObjectWithTracker.SetParent(Tracker,true);
            
            // Save relative local transform of irlObjectWithTracker
            TrackerParameters.IrlObjectWithTrackerPosition = irlObjectWithTracker.localPosition;
            TrackerParameters.IrlObjectWithTrackerRotation = irlObjectWithTracker.localRotation;
        }
        
        public void AlignSpace(float degree)
        {
            originRoomScale.rotation=Quaternion.Euler(new Vector3(0,degree,0));
            TrackerParameters.Degree = degree;
        }
        
        public void AlignSpaceAndRecenter(float degree)
        {
            AlignSpace(degree);
            RecenterTrackerToSpawn();
        }

        public void ToggleTracker(bool state)
        {
            _trackerTrackedPoseDriver.enabled = state;
        }
        
        private void Awake()
        {
            Tracker=targetedTracker;
            _trackerTrackedPoseDriver = Tracker.GetComponentInParent<TrackedPoseDriver>();
            _movableArea = originRoomScale.transform.parent;
            
            SetFilePath();
            
            spawn.SetParent(_movableArea,true);
            
            if(irlObjectWithTracker != null)
            {
                _initialIrlObjectWithTrackerPosition = irlObjectWithTracker.localPosition;
                _initialIrlObjectWithTrackerRotation = irlObjectWithTracker.localRotation;
            }
            
            LoadParameters();
        }

        public void LoadParameters()
        {
            TrackerParameters=GetParametersFromJson(filePath);
            
            originRoomScale.localPosition = TrackerParameters.TrackerOriginOffset;
            originRoomScale.localRotation = Quaternion.Euler(new Vector3(0,TrackerParameters.Degree,0));

            if (irlObjectWithTracker != null)
            {
                ParentObjectWithTrackerUsingParameters();
            }
        }

        public void SaveParameters()
        {
            string dataAsJson = JsonUtility.ToJson(TrackerParameters);
            File.WriteAllText(filePath, dataAsJson);
        }

        public void DeleteParameters()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("File deleted.");
            }
            else
            {
                Debug.Log("No file to delete.");
            }
        }
        
        public Transform Spawn => spawn;
    }
}
