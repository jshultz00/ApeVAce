using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LedgeBehavior : MonoBehaviour
{

    XRGrabInteractable ledge;
    IXRInteractor curController;
    InputDevice inputController;
    XRRayInteractor jankRay;
    MonkeyBehavior curGrabber;
    Transform handModel = null;
    Vector3 ledgePos;
    Vector3 ledgeForward;
    bool grabbed = false;
    private float pressVal;

    public Collider thisCollider;

    private float refractory = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ledge = this.transform.GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            inputController.TryGetFeatureValue(CommonUsages.grip, out pressVal);
            if (pressVal < 0.1f)
            {
                onRelease();
                return;
            }
        }
        if (handModel != null)
        {
            if (grabbed)
            {
                handModel.position = Vector3.MoveTowards(handModel.position, (ledgePos + ledgeForward * 0.1f), 10f * Time.deltaTime);
                if ((curController.transform.position - handModel.position).magnitude >= 1.5f)
                {
                    onRelease();
                }
            }
            else
            {
                handModel.localPosition = Vector3.MoveTowards(handModel.localPosition, Vector3.zero, 20f * Time.deltaTime);
                if (handModel.localPosition == Vector3.zero)
                {
                    handModel = null;
                }
            }
        }
        if(refractory > 0f)
        {
            refractory -= Time.deltaTime;
        }
    }

    public void onHover()
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    public void onExitHover()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    public void onGrab(SelectEnterEventArgs args)
    {
        if (grabbed)
        {
            if(refractory > 0f)
            {
                return;
            }
            onRelease();
        }
        //edge case when we grab the same ledge with other hand.
        if (handModel != null)
        {
            handModel.localPosition = Vector3.zero;
        }
        curController = args.interactorObject;
        jankRay = curController.transform.GetComponent<XRRayInteractor>();
        jankRay.maxRaycastDistance = 0f;
        curGrabber = curController.transform.parent.gameObject.transform.parent.gameObject.GetComponent<MonkeyBehavior>();
        curController.transform.GetComponent<XRController>().SendHapticImpulse(0.5f, 0.2f);
        inputController = curController.transform.GetComponent<XRController>().inputDevice;
        handModel = curController.transform.GetChild(0);
        ledgePos = ledge.GetAttachTransform(args.interactorObject).position;
        ledgeForward = ledge.GetAttachTransform(args.interactorObject).right;
        curGrabber.ledgeGrabbed(curController);
        grabbed = true;
        refractory = 0.5f;
    }

    public void onRelease()
    {
        //some strange occurance happens at the top, causes null reference so insert check
        if (curController != null)
        {
            curController.transform.GetComponent<XRController>().SendHapticImpulse(0.5f, 0.2f);
        }
        curGrabber.releaseReset(curController);
        jankRay.maxRaycastDistance = 1f;
        jankRay = null;
        curController = null;
        curGrabber = null;
        grabbed = false;
        refractory = 0f;
    }
}