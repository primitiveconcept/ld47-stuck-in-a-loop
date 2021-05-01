namespace StuckInALoop
{
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;


    public class Spooks : MonoBehaviour
    {
        private static Spooks _instance;

        public PlayableDirector[] Animations;


        private static Spooks Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<Spooks>();
                return _instance;
            }
        }


        public static PlayableDirector Play(string name)
        {
            PlayableDirector playable = FindPlayable(name);
            playable.Play();
            return playable;
        }


        public static void Stop(string name)
        {
            FindPlayable(name).Stop();
        }
        
        
        public static PlayableDirector FindPlayable(string name)
        {
            PlayableDirector playable;
            for (int i = 0; i < Instance.Animations.Length; i++)
            {
                if (Instance.Animations[i].name == name)
                {
                    return Instance.Animations[i];
                }
            }
            
            return null;
        }
    }
}