using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DisableCamera : MonoBehaviour
{
    private PhotonView PV;
    public GameObject cam;
    public GameObject left;
    public GameObject right;
   // public GameObject interactionManager;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        cam.SetActive(false);
        //left.SetActive(false);
        //right.SetActive(false);
        //interactionManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //replace when PV works again?
        if (PV != null && PV.IsMine)
        {
            Debug.Log("success");
            cam.SetActive(true);
          //  left.SetActive(true);
          //  right.SetActive(true);
            //interactionManager.SetActive(true);
        }
    }
}
