using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombgmPlay : MonoBehaviour
{
    public AudioSource warpSE;
    public AudioSource[] bgms;
    private bool isProcessing = false;

    private IEnumerator PlaySequence()
    {
        if (isProcessing) yield break;

        isProcessing = true;
        warpSE.Play();

        yield return new WaitForSeconds(2f);

        foreach (AudioSource bgm in bgms)
        {
            bgm.Play();
        }

        isProcessing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlaySequence());
        }
    }
}