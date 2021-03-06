﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AtStudio.GGJ2019
{
    public class Catcher : MonoBehaviour
    {
        public HomeShip parent;
        public Transform hand;
        public Transform handCenter;
        public SpriteRenderer rope;
        public Transform light;
        [ReadOnly]
        public Vector3 velocity;
        [ReadOnly]
        public Vector3 toHand;

        [ReadOnly]
        public Vector3 oriLocalUp;
        // hand up in local
        [ReadOnly]
        public Vector3 handUpLocal;
        public Vector3 handUpWorld;
        public float RotateSpeed = 100f;
        public float pushSpeed = 5f;
        public float flySpeed = 2f;
        public float backSpeed = 1f;
        public float backOffset = 0.1f;
        [ReadOnly]
        public Vector3 handOriginOffset;
        public float maxRopeLength = 5f;
        [ReadOnly]
        public Mine temMine;
        public float limitAngle = 180f;

        [ReadOnly]
        public Vector3 preHandPos;
        [ReadOnly]
        public Vector3 realHandVelocity;

        public int playerID;

        public Collider2D handTrigger;

        // hand original position in world
        public Vector3 handOrigiPosWorld
        {
            get
            {
                return transform.TransformPoint(handOriginOffset);
            }
        }

        public enum CatcherState
        {
            None,
            Push,
            Caught,
            Back,
            Fly,
        }
        CatcherState state;

        public void Start()
        {
            handUpLocal = transform.InverseTransformVector(hand.transform.up);
            handUpWorld = hand.transform.up;
            state = CatcherState.None;
            handOriginOffset = hand.transform.localPosition;
            oriLocalUp = transform.InverseTransformVector(hand.transform.up);
        }

        public void Update()
        {
            UpdateControl();
            UpdateHand();
            UpdateRopeAndLight();
        }

        [Button("Refresh")]
        public void Refresh()
        {
            oriLocalUp = transform.InverseTransformVector(hand.transform.up);
            handOriginOffset = hand.transform.localPosition;  
        }

        public void UpdateControl()
        {
            if ( MPlayerManager.Instance.PlayersReady )
            {
                var rotate = MPlayerManager.Instance.RotateCatcher;
                var push = MPlayerManager.Instance.PushCatcher;

                if ( playerID == 2 )
                {
                    rotate = MPlayerManager.Instance.RotateCatcher2;
                    push = MPlayerManager.Instance.PushCatcher2;
                }

                if ( state == CatcherState.None )
                {
                    Rotate(rotate);

                    if (push)
                        Push();
                } else if ( state == CatcherState.Push )
                {
                }
            }
        }

        public void UpdateRopeAndLight()
        {
            if (state == CatcherState.None)
            {
                rope.size = new Vector2(0.3f, 0);
                light.transform.up = toHand.normalized;
            }
            else
            {
                float ropeLength = (handCenter.transform.position - handOrigiPosWorld).magnitude;
                rope.size = new Vector2(0.3f, ropeLength * 2f );
            }


            rope.transform.up = toHand.normalized;

        }

        public void UpdateHand()
        {
            parent.flyTo = Vector3.zero;

            if ( state == CatcherState.None )
            {
                hand.transform.position = handOrigiPosWorld;

                toHand = hand.transform.up;
            }
            else if ( state == CatcherState.Push )
            {
                hand.transform.position = preHandPos + velocity * Time.deltaTime;

                toHand = (hand.transform.position - handOrigiPosWorld);
                hand.transform.up = toHand.normalized;
                //rope.transform.up = toHand.normalized;

                //rope.size = new Vector2(0.2f, toHand.magnitude);

                if (toHand.magnitude > maxRopeLength)
                    Caught();
            }else if ( state == CatcherState.Caught )
            {
                state = CatcherState.Fly;
            }else if ( state == CatcherState.Fly)
            {

                toHand = temMine.transform.position - handOrigiPosWorld;

                
                hand.transform.position = temMine.transform.position;
                //Vector3 backHand = - toHand * backSpeed * Time.deltaTime;
                //backHand = Vector3.ClampMagnitude(backHand, pushSpeed * 2f);

                //toHand *= (1f - backSpeed * Time.deltaTime);
                //hand.transform.position = handOrigiPosWorld + toHand;

                Vector3 flyTo = toHand * flySpeed;

                parent.flyTo = flyTo;

                hand.transform.up = toHand.normalized;

                if (toHand.magnitude < backOffset)
                    Back();
            }
            if (state == CatcherState.Back)
            {

                toHand = preHandPos - handOrigiPosWorld;

                Vector3 backHand = -toHand * backSpeed * Time.deltaTime;
                backHand = Vector3.ClampMagnitude(backHand, pushSpeed * 2f);

                toHand *= (1f - backSpeed * Time.deltaTime);
                hand.transform.position = handOrigiPosWorld + toHand;

                hand.transform.up = toHand.normalized;

                if (toHand.magnitude < backOffset)
                    Back();
            }

            realHandVelocity = (hand.transform.position - preHandPos ) / Time.deltaTime;
            preHandPos = hand.transform.position;

        }

        public void Rotate( float rotate )
        {

            // handUpLocal = Quaternion.AngleAxis(rotate * RotateSpeed * Time.deltaTime, Vector3.forward) * handUpLocal ;
            handUpWorld = Quaternion.AngleAxis(rotate * RotateSpeed * Time.deltaTime, Vector3.back) * handUpWorld;

            //if (limitAngle > 0)
            //{
            //    var angle = Vector3.SignedAngle(oriLocalUp, handUpLocal , Vector3.forward   );
            //    if (angle > limitAngle * 0.5f)
            //        handUpLocal = Quaternion.AngleAxis(limitAngle * 0.5f, Vector3.forward) * oriLocalUp;
            //    if (angle < - limitAngle * 0.5f)
            //        handUpLocal = Quaternion.AngleAxis( - limitAngle * 0.5f, Vector3.forward) * oriLocalUp;

            //}
            //hand.transform.up = transform.TransformVector(handUpLocal);


            //if ( limitAngle > 0 )
            //{
            //    var angle = Vector3.SignedAngle(transform.up, handUpWorld, Vector3.forward);
            //    if (angle > limitAngle * 0.5f)
            //        handUpWorld = Quaternion.AngleAxis(limitAngle * 0.5f, Vector3.forward) * transform.up;
            //    if (angle < -limitAngle * 0.5f)
            //        handUpLocal = Quaternion.AngleAxis(-limitAngle * 0.5f, Vector3.forward) * oriLocalUp;
            //}

            hand.transform.up = handUpWorld;
        }

        public void Push()
        {
            state = CatcherState.Push;

            velocity = hand.transform.up * pushSpeed;
        }

        public void Caught( Mine mine = null )
        {
            if (state == CatcherState.Push)
            {

                if ( mine != null )
                {
                    temMine = mine;
                    temMine.Caught(this);
                    state = CatcherState.Caught;
                }
                else
                {
                    state = CatcherState.Back;
                }
            }
        }

        public void Back()
        {
            if (state == CatcherState.Back || state == CatcherState.Fly)
            {
                Debug.Log("Back");
                state = CatcherState.None;

                if (temMine != null)
                    temMine.Collect(this);

                if (parent != null)
                    parent.CollectMine(temMine);

                temMine = null;
            }
        }
    }
}