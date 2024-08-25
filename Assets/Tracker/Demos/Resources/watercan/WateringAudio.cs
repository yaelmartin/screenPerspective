using UnityEngine;

namespace Tracker.Demos.Resources.watercan
{
    //CC0 Script and sound from https://freesound.org/people/wobesound/sounds/488401/
    public class WateringAudio : MonoBehaviour {
        public float maxVolume = .3f;
        public float fadeInTime = 0.5f;
        public float fadeOutTime = 0.25f;
        float volumeUnreal = 0;
        bool fadeIn = false;
        bool fadeOut = false;
        float nowFade;
        public AudioSource audioSource;
        bool pausedYet = false;
        public float fadeCurve = 2;
        void Start () { audioSource.loop = true;}
        public void Sound (bool action) {
            if (action) {
                fadeIn = true;
                fadeOut = false;
                if (!pausedYet){
                    audioSource.Play();
                    pausedYet = true;
                } else { audioSource.UnPause(); }
            }
            if (!action)
            {
                fadeOut = true;
                fadeIn = false;
            }
            if (fadeIn)
            {
                nowFade = Time.deltaTime * maxVolume / fadeInTime;
                if (volumeUnreal + nowFade < maxVolume) {
                    volumeUnreal += Time.deltaTime * maxVolume / fadeInTime;
                } else {
                    volumeUnreal = maxVolume;
                    fadeIn = false;
                }
                audioSource.volume = Mathf.Pow(volumeUnreal, fadeCurve);
            }
            if (fadeOut) {
                nowFade = Time.deltaTime * maxVolume / fadeInTime;
                if (volumeUnreal > Time.deltaTime * maxVolume / fadeInTime)
                {
                    volumeUnreal -= Time.deltaTime * maxVolume / fadeInTime;
                } else {
                    volumeUnreal = 0;
                    audioSource.Pause();
                    fadeOut = false;
                }
                audioSource.volume = Mathf.Pow(volumeUnreal, fadeCurve);
            }
        }
    }
}
