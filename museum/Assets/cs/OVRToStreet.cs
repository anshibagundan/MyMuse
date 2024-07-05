using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRToStreet : MonoBehaviour
{
    public OVRCameraRig ovrCameraRig;
    public static bool isToStreet = false;
    private float time = 0;
    private Transform centerEyeAnchor;

    private bool canBackStreetByEyeTrack = false;

     //部屋の名前表示
    private RoomNameLeftUpShow roomNameLeftUpShow;


    void Start()
    {
        if (ovrCameraRig == null)
        {
            Debug.LogError("OVRCameraRigが設定されていません。");
            return;
        }

        centerEyeAnchor = ovrCameraRig.centerEyeAnchor;
    }

    private void Update()
    {
        if (isToStreet)
        {
            Drop();
        }

        if (80 < centerEyeAnchor.rotation.eulerAngles.y && centerEyeAnchor.rotation.eulerAngles.y < 100)
        {
            canBackStreetByEyeTrack = true;
        }
        else
        {
            canBackStreetByEyeTrack = false;
        }
    }
    //ぶつかったら
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーだったら
        if (other.gameObject.tag == "toStreet" && !isToStreet && canBackStreetByEyeTrack)
        {
            //RoomNameLeftUpShowの取得
            Transform roomNameLeftUp = other.transform.parent.GetChild(1);
            roomNameLeftUpShow = roomNameLeftUp.GetComponent<RoomNameLeftUpShow>();

            toStreet();
            transform.position += new Vector3(0, 4, 0);
            isToStreet = true;
        }
    }

    private void toStreet()
    {
        //廊下に移動
        transform.position += new Vector3(20, 0, 0);
        //部屋の名前非表示
        roomNameLeftUpShow.Hide();
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

