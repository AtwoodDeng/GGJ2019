using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtStudio.GGJ2019
{
    
    public class HomeShip : MonoBehaviour
    {
        public float RotateSpeed = 100f;
        public float pushForce = 1f;
        public SpriteRenderer rockTex;
        public Transform light;
        public Vector3 rocketUp = Vector3.up;
        public Vector3 lightUp = Vector3.up;
        public Vector3 velocity;
        public float maxSpeed = 2f;
        public Vector3 accelecration;


        public PlayerActions PlayerOneAction
        {
            get
            {
                if ( MPlayerManager.Instance.PlayersReady )
                {
                    return MPlayerManager.Instance.playerOne.Actions;
                }
                return null;
            }
        }

        public PlayerActions PlayerTwoAction
        {
            get
            {
                if (MPlayerManager.Instance.PlayersReady)
                {
                    return MPlayerManager.Instance.playerOne.Actions;
                }
                return null;
            }
        }

        public Rigidbody2D _rigidbody;
        public Rigidbody2D m_rigidbody
        {
            get
            {
                if ( _rigidbody == null )
                {
                    _rigidbody = GetComponent<Rigidbody2D>();
                }
                return _rigidbody;
            }
        }

        public Vector2 FromV3( Vector3 input )
        {
            return new Vector2(input.x, input.y);
        }

        public void Start()
        {
            rocketUp = rockTex == null ? Vector3.up : rockTex.transform.up;
            lightUp = light == null ? Vector3.up: light.transform.up;
        }

        public void Update()
        {

            UpdateControl();
            UpdateVisual();
        }

        public void UpdateControl()
        {
            if ( MPlayerManager.Instance.PlayersReady )
            {
                var rotateship = PlayerOneAction.RotateShip.Value;
                var rotateLight = PlayerTwoAction.RotateCatcher.Value;
                var pushShip = PlayerTwoAction.PushShip;

                rocketUp = Quaternion.AngleAxis(rotateship * RotateSpeed * Time.deltaTime, Vector3.back) * rocketUp;
                lightUp = Quaternion.AngleAxis( rotateLight * RotateSpeed * Time.deltaTime, Vector3.back) * lightUp;


                var vel = m_rigidbody.velocity;
                if ( pushShip )
                {
                    vel += FromV3(rocketUp) * pushForce * Time.deltaTime; 
                }

                vel = Vector3.ClampMagnitude(vel, maxSpeed);

                m_rigidbody.velocity = vel;
            }


        }

        public void UpdateVisual()
        {
            if (rockTex != null)
            {
                rockTex.transform.up = rocketUp;

            }

            if ( light != null )
            {
                light.transform.up = lightUp;
            }
        }


    }

}