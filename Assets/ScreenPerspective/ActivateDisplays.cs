using System;
using UnityEngine;

namespace ScreenPerspective
{
    public class ActivateDisplays : MonoBehaviour
    {
        [SerializeField] private int maxDisplayCount = 8;

        private bool isEditor = false;
        
        void Start()
        {
            #if UNITY_EDITOR
            isEditor = true;
            #endif   
            
            int totalDisplays = Display.displays.Length; //https://docs.unity3d.com/ScriptReference/Display-displays.html
            
            if(isEditor){totalDisplays = maxDisplayCount;} 
            
            // Loop through and activate additional displays if available
            for (int i = 1; i < Math.Min(totalDisplays,8); i++)
            {
                if(!isEditor) {Display.displays[i].Activate();}
            }
        }
    }
}
