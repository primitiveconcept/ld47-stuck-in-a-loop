namespace StuckInALoop
{
    using UnityEngine;


    public class MusicPlayer : MonoBehaviour
    {
        public AudioSource MusicIntro;
        public AudioSource MusicLoop;

        public void Start()
        {
            this.MusicIntro.loop = false;
            this.MusicLoop.loop = true;
            
            this.MusicIntro.Play();
            //this.MusicLoop.PlayDelayed(this.MusicIntro.clip.length);
        }


        public void Update()
        {
            if (!this.MusicIntro.isPlaying
                && !this.MusicLoop.isPlaying)
            {
                this.MusicLoop.Play();
            }
        }

    }
}