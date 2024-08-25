using System.IO;
using UnityEngine;

namespace Tracker
{
    public struct TrackerParameters
    {
        public float Degree;
        public Vector3 TrackerOriginOffset;
        public Vector3 IrlObjectWithTrackerPosition;
        public Quaternion IrlObjectWithTrackerRotation;
    }
    
    /// <summary>
    /// Retrieves calibration to move a virtual object like the real one
    /// </summary>
    public class TrackerConfigLoader : MonoBehaviour
    {
        private protected Transform Tracker;
        [SerializeField] private protected Transform irlObjectWithTracker;
        public string filePath = "trackerParameters.json";
        private protected TrackerParameters TrackerParameters;
        
        private void Awake()
        {
            Tracker = transform;
            SetFilePath();

            
            TrackerParameters=GetParametersFromJson(filePath);
            
            ParentObjectWithTrackerUsingParameters();
        }

        protected void SetFilePath()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, filePath);
        }

        protected static TrackerParameters GetParametersFromJson(string file)
        {
            if (File.Exists(file))
            {
                Debug.Log("File " + file + " found!");
                string dataAsJson = File.ReadAllText(file);
                return JsonUtility.FromJson<TrackerParameters>(dataAsJson);
            }
            else
            {
                Debug.Log("File " + file + " not found. Using default Parameters.");
                return new TrackerParameters();
            }
        }

        protected void ParentObjectWithTrackerUsingParameters()
        {
            if (irlObjectWithTracker == null) return;
            irlObjectWithTracker.SetParent(Tracker);
            irlObjectWithTracker.localPosition = TrackerParameters.IrlObjectWithTrackerPosition;
            irlObjectWithTracker.localRotation = TrackerParameters.IrlObjectWithTrackerRotation;
        }
    }  
}

