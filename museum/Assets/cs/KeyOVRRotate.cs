using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOVRRotate : MonoBehaviour
{
    public OVRCameraRig ovrCameraRig;
    private Transform centerEyeAnchor;
    void Start()
    {
        centerEyeAnchor = ovrCameraRig.centerEyeAnchor;
    }


    void Update()
    {
        if(Input.GetKey(KeyCode.J))
        {
            centerEyeAnchor.Rotate(0, -0.5f, 0);
        }

        if(Input.GetKey(KeyCode.L))
        {
            centerEyeAnchor.Rotate(0, 0.5f, 0);
        }
    }
}
