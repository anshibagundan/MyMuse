using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNameLeftUpShow : MonoBehaviour
{
    private TextMeshPro nameText;
    void Start()
    {
        //カメラ設定
        Canvas canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
        
        nameText = this.GetComponent<TextMeshPro>();
        Hide();
    }

    public void Show(string roomName){
        nameText.text = roomName;
        float alpha = 1.0f;
        var colortemp = nameText.color;

        nameText.color = new Color(colortemp.r, colortemp.g, colortemp.b, alpha);
    }

    public void Hide(){
        float alpha = 0.0f;
        var colortemp = nameText.color;

        nameText.color = new Color(colortemp.r, colortemp.g, colortemp.b, alpha);
    }
}
