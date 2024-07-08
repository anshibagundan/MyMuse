using UnityEngine;

public class zoomCamera : MonoBehaviour
{
    public Camera cameraToZoom; // ズームするカメラ
    public float zoomFactor = 0.1f; // ズーム速度と量
    public float maxZoomIn = 2.0f; // 最大ズームイン倍率
    public float maxZoomOut = 0.5f; // 最大ズームアウト倍率

    void Update()
    {
        // Yボタンでズームイン
        if (Input.GetKeyDown(KeyCode.JoystickButton3)) // OculusのYボタン
        {
            Debug.Log("Ybutton");
            Debug.Log(zoomFactor);
            if (cameraToZoom.fieldOfView > maxZoomOut)
            {
                cameraToZoom.fieldOfView -= zoomFactor;
            }
        }

        // Xボタンでズームアウト
        if (Input.GetKeyDown(KeyCode.JoystickButton2)) // OculusのXボタン
        {
            Debug.Log("Xbutton");
            Debug.Log(zoomFactor);
            if (cameraToZoom.fieldOfView < maxZoomIn)
            {
                cameraToZoom.fieldOfView += zoomFactor;
            }
        }
        
    }
}