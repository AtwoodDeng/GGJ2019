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
        [FoldoutGroup("Control")]
        [ReadOnly]
        public Vector3 flyTo;


        [FoldoutGroup("Heal")]
        public float maxHealth = 150f;
        [FoldoutGroup("Heal")]
        [ReadOnly]
        public float realHealth = 100f;
        [FoldoutGroup("Heal")]
        [ReadOnly]
        public float temHealth = 100f;
        [FoldoutGroup("Heal")]
        public List<LightCombo> lightList = new List<LightCombo>();
        [FoldoutGroup("Heal")]
        AnimationCurve lightCurve = new AnimationCurve();
        [FoldoutGroup("Heal")]
        public float healthReducePreSecond = 3.3f;




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
            temHealth = 100f;
            //lightUp = light == null ? Vector3.up: light.transform.up;
        }

        public void Update()
        {
            UpdateControl();
            UpdateVisual();
            UpdateSound();
            UpdateHealthAndLight();
        }

        [System.Serializable]
        public class LightCombo
        {
            public DynamicLight2D.DynamicLight dynamicLight;
            public Light light;
            public float originDynamicLightIntensity = 1f;
            public float originLightIntensity = 1f;

            public void ApplyIntensity( float rate )
            {
                dynamicLight.Intensity = originDynamicLightIntensity * rate;
                light.intensity = originLightIntensity * rate;
            }

            public LightCombo( DynamicLight2D.DynamicLight dl , Light l )
            {
                dynamicLight = dl;
                light = l;
                originDynamicLightIntensity = dynamicLight.Intensity;
                originLightIntensity = light.intensity;
            }

            [Button]
            public void Refresh()
            {
                originDynamicLightIntensity = dynamicLight.Intensity;
                originLightIntensity = light.intensity;

            }
        }

        public void UpdateHealthAndLight()
        {

            var pushShip = MPlayerManager.Instance.PushShip;
            if ( pushShip)
                temHealth -= healthReducePreSecond * Time.deltaTime;


            realHealth = Mathf.Lerp(realHealth, temHealth, 5f * Time.deltaTime);
            float rate = lightCurve.Evaluate(realHealth / 100f );
            foreach(var lc in lightList )
            {
                lc.ApplyIntensity( rate );
            }
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
                if (MPlayerManager.Instance.PushCatcher.WasPressed)
                {
                    pushCatcherSound.Play(gameObject);
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

                bool isFlyTo = flyTo.magnitude > 0;

                if ( pushShip )
                {
                    vel += FromV3(transform.up) * (isFlyTo? 0.2f : 1f) * pushForce * Time.deltaTime; 

                }

                if ( flyTo.magnitude > 0 )
                {
                    vel += FromV3(flyTo) * Time.deltaTime;
                }

                vel = Vector3.ClampMagnitude(vel, (isFlyTo? 2f : 1f ) * maxSpeed);
                vel = vel * Mathf.Clamp01(1f - drag * Time.deltaTime);

                m_rigidbody.velocity = vel;

                var angleVel = m_rigidbody.angularVelocity;

                angleVel += rotateship * - RotateSpeed;

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
        }

        public void FlyTo( Vector3 toPos )
        {
            var vel = m_rigidbody.velocity;
            vel += FromV3(toPos) * pushForce * Time.deltaTime;

            vel = Vector3.ClampMagnitude(vel, maxSpeed);
            vel = vel * Mathf.Clamp01(1f - drag * Time.deltaTime);

            m_rigidbody.velocity = vel;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Mine"))
            {
                var mine = collision.GetComponent<Mine>();

                if (mine.type != Mine.MineType.Dead)
                {
                    foreach (var catcher in catchers)
                    {
                        if (catcher.handTrigger.IsTouching(collision))
                        {
                            catcher.Caught(mine);
                        }
                    }
                }


            }

        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if ( collision.collider.tag.Equals("Hurt"))
            {

                Debug.Log("Hurt");
            }
        }

        public void CollectMine( Mine mine )
        {
            if ( mine != null )
                temHealth += mine.GetHealth();
        }


    }

}