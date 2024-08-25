using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;

namespace Tracker
{
    /// <summary>
    /// Used to calibrate a irl object and a tracker
    /// </summary>
    public class TrackerOperations : TrackerConfigLoader
    {
        [SerializeField] private Transform targetedTracker;
        [SerializeField] private Transform originRoomScale;
        [SerializeField] private Transform spawn;
        private TrackedPoseDriver _trackerTrackedPoseDriver;
        
        private Vector3 _initialIrlObjectWithTrackerPosition;
        private Quaternion _initialIrlObjectWithTrackerRotation;
        
        public void RecenterTrackerToSpawn()
        {
            Vector3 offset = spawn.transform.position;
            offset = offset - (Tracker.position - originRoomScale.position);
            originRoomScale.position = offset;

            TrackerParameters.TrackerOriginOffset = offset;

            UnParentIrlObjectAndReparent();
        }

        //must be called when the Tracker is Recentered first and hasn't moved since
        public void UnParentIrlObjectAndReparent()
        {
            irlObjectWithTracker.parent = null;
            irlObjectWithTracker.position = _initialIrlObjectWithTrackerPosition;
            irlObjectWithTracker.rotation = _initialIrlObjectWithTrackerRotation;
            irlObjectWithTracker.SetParent(Tracker,true);
            
            //save relative local transform of irlObjectWithTracker
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
            
            SetFilePath();
            
            spawn.parent = null;
            
            if(irlObjectWithTracker != null)
            {
                _initialIrlObjectWithTrackerPosition = irlObjectWithTracker.position;
                _initialIrlObjectWithTrackerRotation = irlObjectWithTracker.rotation;
            }
            
            LoadParameters();
        }

        public void LoadParameters()
        {
            TrackerParameters=GetParametersFromJson(filePath);
            
            originRoomScale.position = TrackerParameters.TrackerOriginOffset;
            originRoomScale.rotation=Quaternion.Euler(new Vector3(0,TrackerParameters.Degree,0));
            
            ParentObjectWithTrackerUsingParameters();
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

    }
}
