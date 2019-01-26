using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtStudio.GGJ2019;

public class ShootingStar : MonoBehaviour {

    private Rigidbody2D rig;
    private Vector3 homeShipPos;
    private Vector3 forceDir = Vector3.zero;

    public float force = 5f;
    public float angle = 15f;

    private void Awake()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        //homeShipPos = GameObject.Find("homeShip").transform.position;
        homeShipPos = HomeShip.Instance.transform.position;
    }
    // Use this for initialization
    void Start () {
        forceDir = homeShipPos - gameObject.transform.position;
        forceDir = Vector3.Normalize(forceDir);
        rig.AddForce(forceDir * force * Random.Range(0.5f,1.5f),ForceMode2D.Impulse);

        transform.localScale *= Random.Range(0.5f, 1.5f);
        rig.mass *= Random.Range(0.5f, 1.5f);

        angle = Random.Range(-1f, 1f);

    }

    private void Update()
    {
        gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), angle);
    }

}
