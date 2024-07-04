using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRoom : MonoBehaviour
{
    public static bool isEnterPicture = false;
    private float time = 0;

    private void Update()
    {
        if (isEnterPicture)
        {
            EnterPictureAnimation();
        }
    }

    //ぶつかったら
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーだったら
        if (other.gameObject.tag == "toRoom" && !isEnterPicture)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.position += new Vector3(2, 0, 0);
            isEnterPicture = true;
        }
    }

    //部屋に移動
    private void toRoom()
    {
        transform.position += new Vector3(-25, 0, 0);
        transform.position = new Vector3(transform.position.x, 6, transform.position.z);
    }

    // 写真に引き込まれる
    private void EnterPictureAnimation()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            isEnterPicture = false;
            time = 0;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            toRoom();
        }
        else
        {
            //5秒間くるくる回るアニメーション
            transform.Rotate(0, 0, 70 * Time.deltaTime);
            transform.position += new Vector3((float)-0.005, (float)0.004, 0);
        }
    }

}
