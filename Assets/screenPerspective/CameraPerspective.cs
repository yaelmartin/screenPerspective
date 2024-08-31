using System;
using UnityEngine;
using UnityEditor;

namespace screenPerspective
{
    public class CameraPerspective : MonoBehaviour
    {
        [SerializeField] private Transform screenScaleMultiplier;
        [SerializeField] private Transform screenCenter;
        [SerializeField] private Transform forFovFront;
    
        [SerializeField] private Transform forLensShiftY;
        [SerializeField] private Transform forLensShiftX;
        [SerializeField] private Transform screenVerticalBorderUp;
        [SerializeField] private Transform screenHorizontalBorderRight;
    
        [SerializeField] private Camera physicalCamera;
        [SerializeField] private Transform eyeTracker;

        [SerializeField] private Transform screenQuad;
        [SerializeField] private float widthCm = 100 * (16/9);
        [SerializeField] private float heightCm = 100;
        
        private float _widthDivisor = 1;
        private float _heightDivisor = 1;

        private float _screenScale;

        public float ScreenScale
        {
            set
            {
                _screenScale = value;
                
            }
        }

        private void Start()
        {
            _screenScale = screenScaleMultiplier.localScale.x;
            CalculateLensShiftDivisors();
        }

        private void CalculateLensShiftDivisors()
        {
            // It is required to know ScreenScaleMultiplier scale, as Lens shift values depend on it
            _heightDivisor = (Vector3.Distance(screenCenter.position, screenVerticalBorderUp.position) * -2) /
                             _screenScale;
            _widthDivisor = (Vector3.Distance(screenCenter.position,screenHorizontalBorderRight.position) * -2) / _screenScale;
        }

        public void ChangeFov()
        {
            forFovFront.localPosition = new Vector3(0,0,physicalCamera.transform.localPosition.z);
        
            physicalCamera.fieldOfView = 2 * GetAngles(forFovFront.position, screenCenter.position, screenVerticalBorderUp.position);
            //Debug.Log($"Angle between AB and BC: {_fov} degrees");
        }
    
        public float GetAngles(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 side1 = p1-p0;
            Vector3 side2 = p2-p0;
            return Vector3.Angle(side1, side2);
        }


        public void ChangeLensShift()
        {
            var cameraTransform = physicalCamera.transform.localPosition;
            forLensShiftY.localPosition = new Vector3( 0,cameraTransform.y, 0);
            forLensShiftX.localPosition = new Vector3( cameraTransform.x,0, 0);
            
            float lensShiftY = forLensShiftY.localPosition.y / _heightDivisor;
            float lensShiftX = forLensShiftX.localPosition.x / _widthDivisor;

            physicalCamera.lensShift = new Vector2(lensShiftX, lensShiftY);
        }
    
        void Update()
        {
            physicalCamera.transform.position = eyeTracker.position;
            ChangeFov();
            
            float newScreenScale = screenScaleMultiplier.localScale.x;
            if (_screenScale != newScreenScale)
            {
                Debug.Log("Screen Scale changed");
                _screenScale = newScreenScale;
                CalculateLensShiftDivisors();
            }
            ChangeLensShift();
        }

        public void ApplyNewSizeEditMode()
        {
            float widthM = widthCm / 100;
            float heightM = heightCm / 100;
            screenQuad.localScale = new Vector3(widthM,heightM,1);
            
            screenVerticalBorderUp.localPosition = new Vector3(0,heightM/2,0);
            screenHorizontalBorderRight.localPosition = new Vector3(widthM/2,0,0);
            
            // Physical camera sensor size is in millimeters
            float ratio = widthM / heightM;
            float heightMm = 10;
            physicalCamera.sensorSize = new Vector2(ratio * heightMm,heightMm);
            
            // Updating once
            Start();
            Update();
        }
    }
    
    [CustomEditor(typeof(CameraPerspective))]
    public class ScreenSizeChangeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CameraPerspective screen = (CameraPerspective)target;
            if (GUILayout.Button("Apply ratio and size"))
            {
                screen.ApplyNewSizeEditMode();
            }
        }
    }
}
