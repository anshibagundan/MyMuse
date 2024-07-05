using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToStreet : MonoBehaviour
{
    public static bool isToStreet = false;
    private float time = 0;
    private void Update()
    {
        if (isToStreet)
        {
            Drop();
        }
    }
    //ぶつかったら
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーだったら
        if (other.gameObject.tag == "toStreet")
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            toStreet();
            transform.position += new Vector3(0, 4, 0);
            isToStreet = true;
        }
    }

    private void toStreet()
    {
        //廊下に移動
        transform.position += new Vector3(20, 0, 0);
    }

    private void Drop()
    {
        time += Time.deltaTime;
        if (time > 0.5)
        {
            isToStreet = false;
            time = 0;
            transform.position = new Vector3(transform.position.x, 6, transform.position.z);
        }
        else
        {
            //落下
            transform.position += new Vector3(0, -8*Time.deltaTime, 0);
        }
    }
}

