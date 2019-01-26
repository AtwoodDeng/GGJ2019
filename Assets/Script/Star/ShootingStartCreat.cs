using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtStudio.GGJ2019;

public class ShootingStartCreat : MonoBehaviour {

    public GameObject shootingStar;

    public float timeInterval = 1f;
    
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateStarRepeatly());
    }

    IEnumerator CreateStarRepeatly()
    {
        float timer = 15f;

        while( true )
        {
            if (MPlayerManager.Instance.PlayersReady)
                timer -= Time.deltaTime;

            if ( timer < 0 )
            {
                CreateStar();

                timer = timeInterval * Random.Range(0.5f, 1.5f);
            }

            yield return null;
        }
    }

    public void CreateStar(  )
    {
        Vector3 pos = GetRandomPosition();
        var star = GameObject.Instantiate(shootingStar, pos, Quaternion.identity);
        star.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //    if (Input.GetKeyDown(KeyCode.J))
        //    {
        //        Vector3 pos = CreatPos();
        //        GameObject.Instantiate(shootingStar, pos, Quaternion.identity);
        //    }
    }

    Vector3 GetRandomPosition()
    {
        float pos_x = Random.Range(-10.0f, 11.0f);
        float pos_y = Random.Range(10.0f, 16.0f);
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.height / 2, Screen.width / 2));
        pos.x += pos_x;
        pos.y += pos_y;
        pos.z = 0f;
        return pos;
    }
}
