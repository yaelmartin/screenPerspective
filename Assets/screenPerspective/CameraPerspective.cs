using UnityEngine;

namespace screenPerspective
{
    public class CameraPerspective : MonoBehaviour
    {
    
        //tree points/Vector3
        [SerializeField] private Transform screenCenter;
        [SerializeField] private Transform screenVerticalBorderUp;
        [SerializeField] private Transform forFovFront;
    
        [SerializeField] private Transform forLensShiftY;
        [SerializeField] private Transform forLensShiftX;
        [SerializeField] private Transform screenHorizontalBorderRight;
    
        [SerializeField] private Camera physicalCamera;
        [SerializeField] private Transform eyeTracker;
        private float _fov=1;


    


        public void CalculateFov()
        {
            forFovFront.localPosition = new Vector3(0,0,physicalCamera.transform.localPosition.z);
        
            _fov = 2 * GetAngles(forFovFront.position, screenCenter.position, screenVerticalBorderUp.position);
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
        
            var centerPosition = screenCenter.position;
        
            float lensShiftY = forLensShiftY.localPosition.y / (Vector3.Distance(centerPosition,screenVerticalBorderUp.position) * - 2);
            float lensShiftX = forLensShiftX.localPosition.x /  (Vector3.Distance(centerPosition,screenHorizontalBorderRight.position) * -2);
        
            Vector2 lensShift = new Vector2(lensShiftX, lensShiftY);

            physicalCamera.lensShift = lensShift;
        }
    
        void Update()
        {
            physicalCamera.transform.position = eyeTracker.position;
            CalculateFov();
            physicalCamera.fieldOfView = _fov;
            ChangeLensShift();
        }
    }
}
