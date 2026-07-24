using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class throttleInteractor : MonoBehaviour
{
    private Transform plane;
    private planeController controller;
    private Transform pivot;
    private Transform interactorOrigin;
    public Transform playerOrigin;
    public Transform lHand;
    public Transform lController;

    private bool grabState = false;


    void Start()
    {
        interactorOrigin = transform.parent;
        plane = interactorOrigin.transform.parent;
        controller = plane.GetComponent<planeController>();
        pivot = plane.Find("throttlePivot");
        Physics.IgnoreCollision(transform.GetComponent<BoxCollider>(), playerOrigin.GetComponent<BoxCollider>(), true);
    }

    public void updatedGrabbed()
    {
        // If we are releasing the interactable, return the hand to the player and restore its position/rotation
        if (grabState)
        {
            grabState = false;
            lHand.SetParent(lController);
            lHand.localPosition = new Vector3(-0.0942f, 0.0793f, 0.088f);
            lHand.localEulerAngles = new Vector3(0, 0, 90);
            lHand.transform.localScale = new Vector3(.02f, .02f, .02f);

        }
        // If we are grabbing the interactable, move the players hand to the item they think they are grabbing. Adjust rotation/position to match.
        else
        {
            grabState = true;
            lHand.transform.localScale = new Vector3(1, 1, 1);
            transform.SetParent(lController);
            transform.position = Vector3.zero;

            lHand.SetParent(pivot);
            lHand.localPosition = new Vector3(0.0754f, 0.1742f, -0.0593f);
            lHand.localEulerAngles = new Vector3(0, 90, 0);
            lHand.transform.localScale = new Vector3(.02f, .02f, .02f);
        }
    }
    public void relocate()
    {
        // Moves interactable to default location and sets plane as parent again
        transform.SetParent(interactorOrigin);
        transform.localPosition = interactorOrigin.position;
        transform.rotation = interactorOrigin.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

        // make sure the interactable is either in the players hand, or in its resting spot (its origin)
        if (grabState)
        {
            transform.position = lController.position;
        }
        else
        {
            transform.position = interactorOrigin.position;
            transform.rotation = interactorOrigin.rotation;
        }

        // rotate the throttle based on the current distance of the interactable from its origin, capped at a certain angle
        float zRot = controller.throttleValue * 7.5f;
        if (zRot > 70) { zRot = 70; } else if (zRot < -70) { zRot = -70; }
        Vector3 newEuler = new Vector3(pivot.localEulerAngles.x, pivot.localEulerAngles.y, -zRot);
        pivot.localEulerAngles = newEuler;
    }
}