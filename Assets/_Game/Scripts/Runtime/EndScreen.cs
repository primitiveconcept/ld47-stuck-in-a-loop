namespace StuckInALoop
{
    using System;
    using Spineless;
    using UnityEngine;
    using UnityEngine.UI;


    public class EndScreen : MonoBehaviour
    {
        public AudioSource PettingSound;
        public Texture2D PetCursor;


        public void OnMouseEnter()
        {
            Cursor.SetCursor(this.PetCursor, Vector2.zero, CursorMode.Auto);
        }


        public void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


        public void Start()
        {
            var fader = GameObject.Find("FaderImage").GetComponent<Image>();
            fader.color = Color.white;
            Coroutines.EaseImageColor(
                target: fader, 
                color: Color.black, 
                duration: 5f,
                callback: () =>
                    {
                        Coroutines.EaseImageColor(
                            target: fader, 
                            color: Color.clear, 
                            duration: 3f, 
                            callback: () => fader.gameObject.SetActive(false));
                    });
        }


        public void Pet()
        {
            if (!this.PettingSound.isPlaying)
            {
                this.PettingSound.pitch = 1 + UnityEngine.Random.Range(-0.1f, 0.2f);
                this.PettingSound.Play();
            }
                
        }
    }
}