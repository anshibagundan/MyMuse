using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public float speed = 4.0f;  // 移動速度

    void Update()
    {
        // Oculusの右コントローラーのジョイスティックからの入力を取得
        Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        
        // 入力に基づいてカメラの位置を更新
        Vector3 move = new Vector3(input.x, 0, input.y) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}