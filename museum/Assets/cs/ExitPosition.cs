using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitPosition : MonoBehaviour
{
    public Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット

    //public int streetNum = 5;
    public GameObject partation;
    void Start()
    {
        // 1秒後にStartMuseumメソッドを呼び出す
        Invoke("MoveExit", 5f);
    }

    void MoveExit()
    {
        //int streetNum = MuseumMaker.streetNum;
        int streetNum = MakeMyMuseum.streetNum + 1;
        
        transform.position += positionOffset*streetNum;

        partation.SetActive(true);

    }
}