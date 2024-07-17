using UnityEngine;

public class KeyMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;  // 前進・後退速度
    public float rotateSpeed = 100.0f;  // 回転速度
    public float minDistance = 0.3f;  // 最小距離

    void Update()
    {
        // キーボードの入力を取得
        float moveVertical = Input.GetAxis("Vertical");
        float rotateHorizontal = Input.GetAxis("Horizontal");

        // 回転の計算
        float rotation = rotateHorizontal * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // 前進・後退の計算
        Vector3 move = transform.forward * moveVertical * moveSpeed * Time.deltaTime;

        // 衝突を検知して移動を防ぐ
        if (!IsCollisionAhead(move, minDistance))
        {
            transform.position += move;
        }
    }

    // 前方に衝突があるかどうかを判定するメソッド
    bool IsCollisionAhead(Vector3 move, float minDistance)
    {
        float distance = move.magnitude;
        Vector3 direction = move.normalized;
        RaycastHit hit;

        // Raycast で前方に衝突があるかをチェック
        if (Physics.Raycast(transform.position, direction, out hit, distance + minDistance))
        {
            return true;
        }

        return false;
    }
}