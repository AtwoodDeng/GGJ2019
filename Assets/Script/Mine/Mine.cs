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
            Special
        }

        public MineType type;

        

        private void Start()
        {
            gameObject.tag = "Mine";
        }

        virtual public void Caught( Catcher catcher )
        {
            Debug.Log("Mine caught by " + catcher);
            transform.parent = catcher.hand.transform;
        }

        virtual public void Collect( Catcher catcher )
        {
            transform.parent = catcher.transform;
            transform.localScale = Vector3.zero;
        }

        virtual public void Release( Catcher catcher )
        {
            transform.parent = null;

        }
    }
}
