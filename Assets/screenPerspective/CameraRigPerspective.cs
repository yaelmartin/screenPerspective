using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ScreenPerspective
{public class CameraRigPerspective : MonoBehaviour
    {
        [SerializeField] private Transform eyePosition;
        [SerializeField] private Camera physicalCamera;
        [SerializeField] private Transform screenMesh;
        [SerializeField] private Transform screenTopEdge;
        [SerializeField] private Transform screenRightEdge;
        [SerializeField] private Transform fovPivot;
        [SerializeField] private Transform shiftYPivot;
        [SerializeField] private Transform shiftXPivot;

        [SerializeField] private float screenWidthM = 16f/9f;
        [SerializeField] private float screenHeightM = 1f;

        private void Update()
        {
            UpdateCameraPosition();
            UpdateFieldOfView();
            UpdateLensShift();
        }
        private void UpdateCameraPosition()
        {
            physicalCamera.transform.position = eyePosition.position;
        }

        private void UpdateFieldOfView()
        {
            fovPivot.localPosition = new Vector3(0f, 0f, physicalCamera.transform.localPosition.z);
            // Field of View Axis: Vertical
            physicalCamera.fieldOfView = 2f * CalculateAngle(fovPivot.localPosition, Vector3.zero, screenTopEdge.localPosition);
        }

        private float CalculateAngle(Vector3 vertex, Vector3 point1, Vector3 point2)
        {
            return Vector3.Angle(point1 - vertex, point2 - vertex);
        }
        
        private void UpdateLensShift()
        {
            Vector3 cameraLocalPosition = physicalCamera.transform.localPosition;
            shiftYPivot.localPosition = new Vector3(0f, cameraLocalPosition.y, 0f);
            shiftXPivot.localPosition = new Vector3(cameraLocalPosition.x, 0f, 0f);

            float lensShiftY = shiftYPivot.localPosition.y / -screenHeightM;
            float lensShiftX = shiftXPivot.localPosition.x / -screenWidthM;

            physicalCamera.lensShift = new Vector2(lensShiftX, lensShiftY);
        }

        public void ApplyScreenDimensions()
        {
            screenMesh.localScale = new Vector3(screenWidthM, screenHeightM, 1f);

            screenTopEdge.localPosition = new Vector3(0f, screenHeightM / 2f, 0f);
            screenRightEdge.localPosition = new Vector3(screenWidthM / 2f, 0f, 0f);

            float aspectRatio = screenWidthM / screenHeightM;
            float sensorHeight = 10f; // mm
            physicalCamera.sensorSize = new Vector2(aspectRatio * sensorHeight, sensorHeight);
            
            Update();
        }
    }
    #if UNITY_EDITOR
    [CustomEditor(typeof(CameraRigPerspective))]
    public class ScreenSizeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Apply Screen Dimensions"))
            {
                var rig = (CameraRigPerspective)target;
                rig.ApplyScreenDimensions();
            }
        }
    }
    #endif
}