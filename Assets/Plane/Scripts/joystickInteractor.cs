using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class joystickInteractor : MonoBehaviour
{
    private Transform plane;
    private Transform gimbal;
    private Transform interactorOrigin;
    public Transform playerOrigin;
    public Transform rHand;
    public Transform rController;

    public bool grabState = false;

    void Start()
    {
        interactorOrigin = transform.parent;
        plane = interactorOrigin.transform.parent;
        gimbal = plane.Find("joystickGimbal");
        Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), playerOrigin.GetComponent<BoxCollider>(), true);

    }
    public void updatedGrabbed()
    {

        // If we are releasing the interactable, return the hand to the player and restore its position/rotation
        if (grabState)
        {
            grabState = false;
            rHand.SetParent(rController);
            rHand.localPosition = new Vector3(0.149f, 0.079f, 0.088f);
            rHand.localEulerAngles = new Vector3(0, 180, -90);
            rHand.transform.localScale = new Vector3(-.02f, -.02f, -.02f);

            transform.SetParent(interactorOrigin);
            transform.localPosition = Vector3.zero;
            transform.rotation = interactorOrigin.rotation;

        }
        // If we are grabbing the interactable, move the players hand to the item they think they are grabbing. Adjust rotation/position to match.
        else
        {
            grabState = true;
            transform.SetParent(rController);
            transform.localPosition = Vector3.zero;

            rHand.SetParent(gimbal);
            rHand.localPosition = new Vector3(0.1211f, -0.0421f, 0.223f);
            rHand.localEulerAngles = new Vector3(-90, 180, -90);
            rHand.transform.localScale = new Vector3(-.02f, -.02f, -.02f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotates gimbal to face interactable
        // Determine which direction to rotate towards
        Vector3 targetDirection = transform.position - gimbal.position;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(gimbal.forward, targetDirection, 10 * Time.deltaTime, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        gimbal.rotation = Quaternion.LookRotation(newDirection);

    }
}