using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNameLeftUpShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //カメラ設定
        Canvas canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
        
        Hide();
    }

    public void Show(string roomName){
        this.GetComponent<TextMeshPro>().text = roomName;
        this.gameObject.SetActive(true);
    }

    public void Hide(){
        this.gameObject.SetActive(false);
    }
}
