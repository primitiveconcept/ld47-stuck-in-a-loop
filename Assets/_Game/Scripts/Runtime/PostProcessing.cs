namespace StuckInALoop
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;


    public class PostProcessing : MonoBehaviour
    {
        private static PostProcessing _instance;

        public List<PostProcessProfile> Filters;

        private PostProcessVolume _postProcessVolume;


        public static PostProcessProfile CurrentFilter
        {
            get
            {
                return Instance.PostProcessVolume.profile;
            }
        }


        private PostProcessVolume PostProcessVolume
        {
            get
            {
                if (this._postProcessVolume == null)
                    this._postProcessVolume = GetComponent<PostProcessVolume>();
                return this._postProcessVolume;
            }
        }


        private static PostProcessing Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PostProcessing>();
                return _instance;
            }
        }


        public static void SetFilter(string name)
        {
            foreach (PostProcessProfile filter in Instance.Filters)
            {
                if (filter.name == name)
                {
                    Instance.PostProcessVolume.profile = filter;
                    return;
                }
            }
        }


        public static void ClearFilter()
        {
            Instance.PostProcessVolume.profile = null;
        }
    }
}