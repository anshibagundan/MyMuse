using Unity.VisualScripting;
using UnityEngine;

public class OVRToRoom : MonoBehaviour
{
    public static bool isEnterPicture = false;
    private float time = 0;
    public OVRCameraRig ovrCameraRig;
    private Transform centerEyeAnchor;
    private bool canEnterByEyeTrack = false;

    private float maxAngle = -80f;
    private float minAngle = -100f;
    public float rotateSpeed = 70f;

    //部屋の名前表示
    private RoomNameCenterShow roomNameCenterShow;

    void Start()
    {
        if (ovrCameraRig == null)
        {
            Debug.LogError("OVRCameraRigが設定されていません。");
            return;
        }

        centerEyeAnchor = ovrCameraRig.centerEyeAnchor;
    }

    void Update()
    {
        if (isEnterPicture)
        {
            EnterPictureAnimation();
        }



        if (centerEyeAnchor != null)
        {
            // Y軸周りの回転角度を取得（0から360度）
            float yRotation = centerEyeAnchor.eulerAngles.y;

            // 270度から360度の範囲を-90度から0度に変換
            if (yRotation > 180)
            {
                yRotation -= 360;
            }

            // 指定範囲内にあるかチェック
            canEnterByEyeTrack = (yRotation >= minAngle && yRotation <= maxAngle);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "toRoom" && !isEnterPicture && canEnterByEyeTrack)
        {
            float eyeTrackDiff = centerEyeAnchor.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, -90 - eyeTrackDiff, 0);
            transform.position += new Vector3(2, 0, 0);
            isEnterPicture = true;

            //RoomNameCenterShowの取得
            Transform roomNameCenter = other.transform.parent.GetChild(11);
            roomNameCenterShow = roomNameCenter.GetComponent<RoomNameCenterShow>();
        }
    }

    //部屋に移動
    private void toRoom()
    {
        transform.position += new Vector3(-25, 0, 0);
        transform.position = new Vector3(transform.position.x, 6, transform.position.z);
        //部屋の名前表示
        StartCoroutine(roomNameCenterShow.GraduallyShow());
    }

    private void EnterPictureAnimation()
    {
        if (centerEyeAnchor == null)
            return;

        time += Time.deltaTime;
        if (time > 2)
        {
            isEnterPicture = false;
            time = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            toRoom();
        }
        else
        {
            transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
            transform.position += new Vector3(-0.005f, 0.004f, 0);
        }
    }
}