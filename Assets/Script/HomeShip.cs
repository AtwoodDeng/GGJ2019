using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AtStudio.GGJ2019
{
    
    public class HomeShip : MonoBehaviour
    {

        public static HomeShip _instance;
        public static HomeShip Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<HomeShip>();

                }
                return _instance;
            }
        }

        [FoldoutGroup("Main")]
        public SpriteRenderer rockTex;
        [FoldoutGroup("Main")]
        public Catcher[] catchers;
        //[FoldoutGroup("Main")]
        //public Transform light;
        [FoldoutGroup("Control")]
        public float RotateSpeed = 100f;

        [FoldoutGroup("Control")]
        public float pushForce = 1f;
        [FoldoutGroup("Control")]
        public float drag = 1f;
        [FoldoutGroup("Control")]
        public float rotateDrag = 1f;


        [FoldoutGroup("Control")][ReadOnly]
        public Vector3 rocketUp = Vector3.up;
        //[FoldoutGroup("Control")]
        //[ReadOnly]
        //public Vector3 lightUp = Vector3.up;
        [FoldoutGroup("Control")]
        [ReadOnly]
        public Vector3 velocity;
        [FoldoutGroup("Control")]
        public float maxSpeed = 2f;
        [FoldoutGroup("Control")]
        public float maxAngleSpeed = 15f;
        [FoldoutGroup("Control")]
        [ReadOnly]
        public Vector3 accelecration;
        [FoldoutGroup("Control")]
        [ReadOnly]
        public Vector3 catchPoint;

        [FoldoutGroup("Audio")]
        public AudioInfo rotateShipSound = new AudioInfo();
        [FoldoutGroup("Audio")]
        public AudioInfo pushShipSound = new AudioInfo();
        [FoldoutGroup("Audio")]
        public AudioInfo rotateLightSound = new AudioInfo();
        [FoldoutGroup("Audio")]
        public AudioInfo rotateCatcherSound = new AudioInfo();
        [FoldoutGroup("Audio")]
        public AudioInfo pushCatcherSound = new AudioInfo();



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
            //lightUp = light == null ? Vector3.up: light.transform.up;
        }

        public void Update()
        {
            UpdateControl();
            UpdateVisual();
            UpdateSound();
        }

        public void UpdateSound()
        {
            if ( MPlayerManager.Instance.PlayersReady )
            {
                if ( MPlayerManager.Instance.RotateShip.WasPressed )
                {
                    rotateShipSound.Play( gameObject);
                }
                if ( MPlayerManager.Instance.RotateCatcher.WasPressed)
                {
                    rotateCatcherSound.Play(gameObject);
                }
                if ( MPlayerManager.Instance.PushShip.WasPressed)
                {
                    pushShipSound.Play(gameObject);
                }
            }
        }

        public void UpdateControl()
        {
            if ( MPlayerManager.Instance.PlayersReady )
            {
                var rotateship = MPlayerManager.Instance.RotateShip.Value;
                var pushShip = MPlayerManager.Instance.PushShip;
                //var rotateLight = MPlayerManager.Instance.RotateCatcher.Value;

                //rocketUp = Quaternion.AngleAxis(rotateship * RotateSpeed * Time.deltaTime, Vector3.back) * rocketUp;
                //lightUp = Quaternion.AngleAxis( rotateLight * RotateSpeed * Time.deltaTime, Vector3.back) * lightUp;

                var vel = m_rigidbody.velocity;
                if ( pushShip )
                {
                    vel += FromV3(transform.up) * pushForce * Time.deltaTime; 
                }

                vel = Vector3.ClampMagnitude(vel, maxSpeed);
                vel = vel * Mathf.Clamp01(1f - drag * Time.deltaTime);

                m_rigidbody.velocity = vel;

                var angleVel = m_rigidbody.angularVelocity;

                angleVel += rotateship * RotateSpeed;

                angleVel = Mathf.Clamp(angleVel, -maxAngleSpeed, maxAngleSpeed);
                angleVel = angleVel * Mathf.Clamp01(1f - rotateDrag * Time.deltaTime);

                m_rigidbody.angularVelocity = angleVel;
            }


        }

        public void UpdateVisual()
        {
            if (rockTex != null)
            {
                rockTex.transform.up = rocketUp;

            }

            //if ( light != null )
            //{
            //    light.transform.up = lightUp;
            //}
        }



        public void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("On Trigger Enter");
            if (collision.tag.Equals("Mine"))
            {
                var mine = collision.GetComponent<Mine>();
                
                foreach( var catcher in catchers )
                {
                    if ( catcher.handTrigger.IsTouching( collision ) )
                    {
                        catcher.Caught(mine);
                    }
                }


            }

        }


    }

}