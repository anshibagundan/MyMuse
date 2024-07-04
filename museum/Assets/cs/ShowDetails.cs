using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDetails : MonoBehaviour
{
    public GameObject detailPanel;
    private bool isShow = false;
    public void onClick(){
        if(!isShow){
            detailPanel.SetActive(true);
            isShow = true;
        }
        else{
            detailPanel.SetActive(false);
            isShow = false;
        }
    }
}
