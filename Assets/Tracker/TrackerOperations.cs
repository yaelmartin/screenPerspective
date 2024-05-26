using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tracker
{
    public struct TrackerParameters
    {
        public float Degree;
        public Vector3 TrackerOriginOffset;

        public TrackerParameters(int dummyInt)
        {
            Degree = 0;
            TrackerOriginOffset = Vector3.zero;
        }
    }
    public class TrackerOperations : MonoBehaviour
    {
        [SerializeField] private Transform trackerOrigin;
        [SerializeField] private Transform tracker;
        [SerializeField] private Transform spawn;
        [SerializeField] private ActionBasedController trackerActionBasedController;
        
        private string _filePath = Path.Combine(Application.streamingAssetsPath, "trackerParameters.json");
        private TrackerParameters _trackerParameters;
        
        public void RecenterTrackerToSpawn()
        {
            Vector3 offset = spawn.transform.position;
            offset = offset - (tracker.position - trackerOrigin.position);
            trackerOrigin.position = offset;
            _trackerParameters.TrackerOriginOffset = offset;
        }
        
        public void AlignSpace(float degree)
        {
            trackerOrigin.rotation=Quaternion.Euler(new Vector3(0,degree,0));
            _trackerParameters.Degree = degree;
            RecenterTrackerToSpawn();
        }

        public void ToggleTracker(bool state)
        {
            trackerActionBasedController.enabled = state;
        }


        public void Awake()
        { 
            LoadParameters();
        }
        
        public void LoadParameters()
        {
            if (File.Exists(_filePath))
            {
                string dataAsJson = File.ReadAllText(_filePath);
                TrackerParameters parameters = JsonUtility.FromJson<TrackerParameters>(dataAsJson);
                _trackerParameters = parameters;
            }
            else
            {
                Debug.Log("File trackerParameters.json not found. Using default Parameters.");
                _trackerParameters = new TrackerParameters();
            }
            
            ApplyParameters();
        }

        public void ApplyParameters()
        {
            trackerOrigin.position = _trackerParameters.TrackerOriginOffset;
            trackerOrigin.rotation=Quaternion.Euler(new Vector3(0,_trackerParameters.Degree,0));
        }
        
        public void SaveParameters()
        {
            string dataAsJson = JsonUtility.ToJson(_trackerParameters);
            File.WriteAllText(_filePath, dataAsJson);
        }

        public void DeleteParameters()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
                Debug.Log("File deleted.");
            }
            else
            {
                Debug.Log("No file to delete.");
            }
        }

    }
}
