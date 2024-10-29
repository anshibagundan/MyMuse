using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPartation : MonoBehaviour
{
    public GameObject endText;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            endText.SetActive(true);
        }
    }
}
