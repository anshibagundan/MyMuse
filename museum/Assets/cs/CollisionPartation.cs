using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPartation : MonoBehaviour
{
    public GameObject endText;
    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトがボールであるか確認（タグを使用する）
        if (collision.gameObject.CompareTag("Player"))
        {
            endText.SetActive(true);
        }
    }
}
