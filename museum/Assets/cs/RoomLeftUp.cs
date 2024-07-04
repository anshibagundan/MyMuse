using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class RoomLeftUp : MonoBehaviour
{
    void Start(){
        isHide();
    }
    
    public void isShow(){
        this.gameObject.SetActive(true);
    }

    public void isHide(){
        this.gameObject.SetActive(false);
    }
}
