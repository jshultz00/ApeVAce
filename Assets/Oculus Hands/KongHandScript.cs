using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class KongHandScript : MonoBehaviour
{
    public InputDevice curController;
    private Animator anim;
    private float pressVal;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.transform.GetChild(0).GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //Strange workaround, it seems XRController doesn't immediately set inputDevice
        //So we have to call it in update. Not sure if there's a method to fix.
        if(curController != null)
        {
            curController = this.transform.GetComponent<XRController>().inputDevice;
        }
        curController.TryGetFeatureValue(CommonUsages.grip, out pressVal);
        anim.SetFloat("Gripper", pressVal);
    }
}
