using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtStudio.GGJ2019
{
    public class CamSmoothFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 vel;
        public float smoothTime = 1f;

        public void Start()
        {
            vel = Vector3.zero;
        }

        public void Update()
        {
            Vector3 targetPos = target.transform.position;
            targetPos.z = -10f;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, smoothTime);
        }
    }

}