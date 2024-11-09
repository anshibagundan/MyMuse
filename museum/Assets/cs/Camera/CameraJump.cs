using UnityEngine;

public class CameraJump : MonoBehaviour
{
    public OVRCameraRig cameraRig;

    private bool isJumping = false;

    // ジャンプ関連のパラメータ
    private float jumpHeight = 1.5f;
    private float jumpDuration = 0.5f;
    private float jumpStartTime;
    private float initialHeight;

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Three)) && !isJumping && !OVRToRoom.isEnterPicture && !OVRToStreet.isToStreet)

        {
            StartJump();
        }

        if (isJumping)
        {
            UpdateJump();
        }
    }

    private void StartJump()
    {
        isJumping = true;
        jumpStartTime = Time.time;
        initialHeight = cameraRig.transform.position.y;
    }

    private void UpdateJump()
    {
        float elapsedTime = Time.time - jumpStartTime;
        float progress = elapsedTime / jumpDuration;

        if (progress < 1f)
        {
            // 放物線的なジャンプの動き
            float height = initialHeight + jumpHeight * Mathf.Sin(progress * Mathf.PI);
            cameraRig.transform.position = new Vector3(
                cameraRig.transform.position.x,
                height,
                cameraRig.transform.position.z
            );
        }
        else
        {
            // ジャンプ終了時
            cameraRig.transform.position = new Vector3(
                cameraRig.transform.position.x,
                initialHeight,
                cameraRig.transform.position.z
            );
            isJumping = false;
        }
    }
}