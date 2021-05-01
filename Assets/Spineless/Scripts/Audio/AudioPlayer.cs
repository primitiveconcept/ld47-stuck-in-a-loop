namespace Spineless
{
    using UnityEngine;


    public class AudioPlayer : MonoBehaviour
    {
        private static AudioPlayer _instance;
        private AudioSource audioSource;


        private static AudioPlayer instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AudioPlayer>();
                if (_instance == null)
                {
                    GameObject cameraGameObject = Camera.main.gameObject;
                    _instance = cameraGameObject.AddComponent<AudioPlayer>();
                    _instance.audioSource = cameraGameObject.AddComponent<AudioSource>();
                }

                return _instance;
            }
        }


        public static void Play(AudioClip clip, bool pitchVariation = true)
        {
            if (pitchVariation)
                instance.audioSource.pitch = Random.Range(0.90f, 1.1f);
            else
                instance.audioSource.pitch = 1f;
            
            instance.audioSource.PlayOneShot(clip);
        }


        public void PlayEvent(AudioClip clip)
        {
            instance.audioSource.PlayOneShot(clip);
        }
    }
}