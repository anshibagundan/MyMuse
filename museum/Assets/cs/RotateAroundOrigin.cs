using UnityEngine;

public class RotateAroundOrigin : MonoBehaviour
{
    public float rotationSpeed = 10f; // 回転速度

    void Update()
    {
        // 原点を中心にy軸を回転
        transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

