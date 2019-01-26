using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtStudio.GGJ2019
{
    public class Mine : MonoBehaviour
    {

        public enum MineType
        {
            Normal,
            Special,
            Dead,
        }

        public MineType type;
        public float healthGain = 20f;
        

        private void Start()
        {
            gameObject.tag = "Mine";
        }

        virtual public void Caught( Catcher catcher )
        {
            Debug.Log("Mine caught by " + catcher);

            var collider = gameObject.GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;
            //transform.parent = catcher.hand.transform;
        }

        virtual public void Collect( Catcher catcher )
        {
            //transform.parent = catcher.transform;
            transform.localScale = Vector3.zero;
            type = MineType.Dead;
            var collider = gameObject.GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;
            // gameObject.active = false;
        }

        virtual public void Release( Catcher catcher )
        {
            transform.parent = null;

        }

        virtual public float GetHealth()
        {
            return healthGain;
        }
    }
}
