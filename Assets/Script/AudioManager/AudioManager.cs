using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtStudio.GGJ2019
{
    public class AudioManager : MonoBehaviour
    {


        public static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<AudioManager>();

                }
                return _instance;
            }
        }

        public void OnEnable()
        {
            EventManager.Instance.RegistersEvent(EventDefine.PlaySound, PlayClip);
        }

        public void OnDisable()
        {
            EventManager.Instance.UnregistersEvent(EventDefine.PlaySound, PlayClip);

        }

        public void PlayClip( Message msg )
        {
            var info = (AudioInfo)msg.GetMessage("info");
            var parent = (GameObject)msg.GetMessage("parent");

            StartCoroutine(PlayAudio(info, parent));
        }

        IEnumerator PlayAudio( AudioInfo info , GameObject parent )
        {
            var root = parent;
            if (root == null)
                root = gameObject;

            var source = root.AddComponent<AudioSource>();

            var length = info.Apply(source);
            source.Play();
            source.spatialBlend = 0;

            yield return new WaitForSeconds(length);

            DestroyImmediate(source);
            
        }
    }

    [System.Serializable]
    public class AudioInfo
    {
        public AudioClip clip;
        public bool loop;
        public float volume= 1f;

        public float Apply(AudioSource source )
        {
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            if ( clip != null )
            return clip.length;

            return 0;
        }

        public void Play( GameObject parent = null )
        {
            var msg = new Message();
            msg.AddMessage("info", this);
            msg.AddMessage("parent", parent);
            EventManager.Instance.PostEvent(EventDefine.PlaySound, msg);
        }
    }
}