using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombgmStop : MonoBehaviour
{
    public AudioSource[] bgms;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (AudioSource bgm in bgms)
            {
                bgm.Stop();
            }
        }
    }
}
