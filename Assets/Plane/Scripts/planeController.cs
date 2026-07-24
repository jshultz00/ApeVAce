using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class planeController : MonoBehaviour {
    private PhotonView PV;
    private CharacterController myCC;
    private float curVel = 0;
    private static float acceleration = .01f;
    private static float maxSpeed = 20;
    private static float maxThrottle = 20;

    public float throttleValue;

    private Vector3 joystickInteractablePosition;
    private Vector3 throttleInteractablePosition;

    //Variables for weaponry
    private static int wepDmg = 1;
    private static float heatIncrease = 0.025f;
    private static float heatDecrease = 0.2f;
    private float curHeat = 0f;
    private static float maxHeat = 1f;
    private static float fireRate = 20; // how many shots per second
    private float fireTimer = 1 / fireRate;
    private bool overheated = false;

    private List<InputDevice> leftControllerDevice = new List<InputDevice>();
    private List<InputDevice> rightControllerDevice = new List<InputDevice>();

    private Rigidbody planeBody;
    private Transform joystickInteractableOrigin;
    private Transform joystickInteractable;
    private joystickInteractor joystickInteractorScript;

    private Transform throttleInteractableOrigin;
    private Transform throttleInteractable;

    private Transform model;
    private Transform propellor;
    private Transform speedometer;
    private Transform heatometer;

    private float respawnTimer = 0;

    public AudioSource planeExplosionSource;
    public AudioSource planeEngineSource;
    public AudioSource planeWindSource;


    public GameObject projectile = null;

    //Spawns prefab projectile with force of 40 in the plane's current direction.
    private void shoot(InputDevice device) {
        fireTimer += Time.deltaTime;
        if (fireTimer > 1 / fireRate) {
            fireTimer -= 1 / fireRate;
            if (!overheated) {
                //Give it life
                GameObject newProjectile = Instantiate(projectile, planeBody.position + planeBody.transform.forward * 1.5f, planeBody.rotation);
                //Give it purpose
                newProjectile.GetComponent<Rigidbody>().AddForce(planeBody.transform.forward * 40, ForceMode.Impulse);
                newProjectile.GetComponent<ProjectileScript>().setImpactDamageOfWeapon(wepDmg);
                curHeat += heatIncrease;
                device.SendHapticImpulse(0u, 1, .1f);
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        myCC = GetComponent<CharacterController>();
        // Allows us to edit the rotation and movement of the plane
        planeBody = GetComponent<Rigidbody>();

        joystickInteractableOrigin = transform.Find("joystickInteractableOrigin");
        joystickInteractable = joystickInteractableOrigin.transform.Find("joystickInteractable");

        throttleInteractableOrigin = transform.Find("throttleInteractableOrigin");
        throttleInteractable = throttleInteractableOrigin.transform.Find("throttleInteractable");

        joystickInteractorScript = joystickInteractable.GetComponent<joystickInteractor>();

        model = transform.Find("biplane");
        propellor = model.Find("propellor");
        speedometer = model.Find("speedDial").Find("pointerBase");
        heatometer = model.Find("heatDial").Find("pointerBase");

    }

    // Update is called once per frame
    void Update() {
        if (PV.IsMine) {
            if (leftControllerDevice.Count == 0) {
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, leftControllerDevice);
            }

            // Mighty scuffed, but it sure does detect whether or not youre pressing a button. Needs to update every frame because start does not capture the devices for some reason
            if (rightControllerDevice.Count == 0) {
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightControllerDevice);
            }
            else {

                // Detects if A button is pressed on right controller
                rightControllerDevice[0].TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
                if (primaryButtonValue) {

                    // makes sure that the joystick is grabbed before shooting
                    if (joystickInteractorScript.grabState) {
                        shoot(rightControllerDevice[0]);
                    }
                }
            }

            // Finds the position of the interactable relative to the interactable's original position
            joystickInteractablePosition = joystickInteractable.InverseTransformPoint(joystickInteractableOrigin.position);

            // Finds the position of the throttle interactable relative to the interactable's original position
            throttleInteractablePosition = throttleInteractable.InverseTransformPoint(throttleInteractableOrigin.position);

            // Convert the z component of the throttle position into a "throttle value" that determines acceleration
            // Then caps that value to +- 10
            // Problem: the direction of z changes as the plane rotates, meaning an upside down plane will flip the direction of the throttle value
            // solution: Make the interactable the parent of the hand that grabs it, following the rotation of the plane (since the hand is a child of the plane) I wonder what else this could fix
            throttleValue = -throttleInteractablePosition.z * 1.5f;
            if (throttleValue > maxThrottle) { throttleValue = maxThrottle; } else if (throttleValue < -maxThrottle) { throttleValue = -maxThrottle; }

            // Only detect throttle value above an arbritrary threshold, so players can more easily achieve zero
            if (Mathf.Abs(throttleValue) < .05) { throttleValue = 0; }

            // add acceleration to the current velocity, influenced by how high the throttle is
            curVel += acceleration * throttleValue;

            // implement speed cap
            if (curVel > maxSpeed) { curVel = maxSpeed; } else if (curVel < 0) { curVel = 0; }

            // Add velocity to the rigid body velocity
            planeBody.linearVelocity = curVel * transform.forward;

            // makes the rotation speed a factor of the velocity of the plane. Slower plane = slower turns
            float speedBasedRotationFactor = 10 * planeBody.linearVelocity.magnitude / 10;

            // Applies the distance from the origin to the rotation of the plane. Further from origin = faster rotation
            transform.Rotate(joystickInteractablePosition.x * speedBasedRotationFactor * Time.deltaTime * new Vector3(0, 0, 1));
            transform.Rotate(joystickInteractablePosition.z * speedBasedRotationFactor * Time.deltaTime * new Vector3(-1, 0, 0));

            // animates the propellor as a function of velocity.
            propellor.Rotate(planeBody.linearVelocity.magnitude * 10, 0, 0);

            //If the heat exceeds maxheat, wait until heat reaches zero again before they can fire

            if (curHeat > maxHeat) {
                overheated = true;
            }

            if (curHeat > 0f) {
                curHeat -= heatDecrease * Time.deltaTime;
            }
            else {
                curHeat = 0f;
                overheated = false;
            }

            // Updating our lil dials! Would be better to put this stuff in its own script, but then I'd have to make 2 more scripts and I don't like the bloat. So I'll bloat here!
            heatometer.localEulerAngles = new Vector3(heatometer.localEulerAngles.x, 180 - curHeat * maxHeat * 180f, heatometer.localEulerAngles.z);
            speedometer.localEulerAngles = new Vector3(speedometer.localEulerAngles.x, 180 - planeBody.linearVelocity.magnitude * maxSpeed / 2.13f, speedometer.localEulerAngles.z);


            // No time to do this properly, so just throwin the respawn code in here. The timer wont update right unless in update()
            // lots of repeat code but oh well
            Transform smokeObject = model.Find("smoke");
            if (respawnTimer > 0) {
                respawnTimer -= Time.deltaTime;

                // Randomly make smoke dissipate over time
                foreach (Transform smokeSphere in smokeObject) {
                    MeshRenderer smokeMesh = smokeSphere.GetComponent<MeshRenderer>();
                    Material smokeMat = smokeMesh.material;
                    Color smokeColor = smokeMat.color;
                    smokeColor.a -= Random.Range(0, .005f);
                    smokeMat.color = smokeColor;
                }

            }
            else if(respawnTimer < 0) {
                // Resets plane position
                transform.position = new Vector3(0, 1, 0);
                transform.localEulerAngles = new Vector3(0, 0, 0);

                // makes the plane visible again
                foreach (Transform planePart in model) {
                    MeshRenderer partMesh = planePart.GetComponent<MeshRenderer>();
                    partMesh.enabled = true;
                    // Makes all children of parts visible. This is for the dials and propellor
                    foreach (Transform partChild in planePart) {
                        MeshRenderer childMesh = partChild.GetComponent<MeshRenderer>();
                        childMesh.enabled = true;

                        //Surely theres a better way to do this, but its 5:00 am
                        // Redoes the above to get the dial pointer mesh
                        foreach (Transform partGrandchild in partChild) {
                            MeshRenderer grandchildMesh = partGrandchild.GetComponent<MeshRenderer>();
                            grandchildMesh.enabled = true;
                        }
                    }

                }

                // Makes the joystick/throttle visible as well
                Transform joystickGimbal = transform.Find("joystickGimbal");
                Transform throttlePivot = transform.Find("throttlePivot");
                foreach (Transform joystickPart in joystickGimbal) {
                    MeshRenderer childMesh = joystickPart.GetComponent<MeshRenderer>();
                    childMesh.enabled = true;
                }
                foreach (Transform throttlePart in throttlePivot) {
                    MeshRenderer childMesh = throttlePart.GetComponent<MeshRenderer>();
                    childMesh.enabled = true;
                }

                // Makes the smoke invisible
                foreach (Transform smokeSphere in smokeObject) {
                    MeshRenderer smokeMesh = smokeSphere.GetComponent<MeshRenderer>();
                    smokeMesh.enabled = false;
                }

                // Resets the opacity of each smoke cloud
                foreach (Transform smokeSphere in smokeObject) {
                    MeshRenderer smokeMesh = smokeSphere.GetComponent<MeshRenderer>();
                    Material smokeMat = smokeMesh.material;
                    Color smokeColor = smokeMat.color;
                    smokeColor.a = 1;
                    smokeMat.color = smokeColor;
                }

                respawnTimer = 0;
            }
        }

        // Makes the engine volume a function of speed.
        planeEngineSource.volume = planeBody.linearVelocity.magnitude / 20;

        // Make wind sound a function of speed, but only above a certain speed
        planeWindSource.volume = planeBody.linearVelocity.magnitude / 10 - .5f;
        planeWindSource.pitch = planeBody.linearVelocity.magnitude / 10;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Projectile") {
            return;
        }

        curVel = 0;

        // makes the explosion sound
        planeExplosionSource.Play();
        // makes the plane invisble upon a collision
        foreach (Transform planePart in model) {
            MeshRenderer partMesh = planePart.GetComponent<MeshRenderer>();
            partMesh.enabled = false;
            // Makes all children of parts invisble. This is for the dials and propellor
            foreach (Transform partChild in planePart) {
                MeshRenderer childMesh = partChild.GetComponent<MeshRenderer>();
                childMesh.enabled = false;

                //Surely theres a better way to do this, but its 5:00 am
                // Redoes the above to get the dial pointer mesh
                foreach (Transform partGrandchild in partChild) {
                    MeshRenderer grandchildMesh = partGrandchild.GetComponent<MeshRenderer>();
                    grandchildMesh.enabled = false;
                }
            }

        }

        // Makes the joystick/throttle invisble as well
        Transform joystickGimbal = transform.Find("joystickGimbal");
        Transform throttlePivot = transform.Find("throttlePivot");
        foreach (Transform joystickPart in joystickGimbal) {
            MeshRenderer childMesh = joystickPart.GetComponent<MeshRenderer>();
            childMesh.enabled = false;
        }
        foreach (Transform throttlePart in throttlePivot) {
            MeshRenderer childMesh = throttlePart.GetComponent<MeshRenderer>();
            childMesh.enabled = false;
        }


        // Makes the smoke visible upon a collision
        Transform smokeObject = model.Find("smoke");
        foreach (Transform smokeSphere in smokeObject) {
            MeshRenderer smokeRenderer = smokeSphere.GetComponent<MeshRenderer>();
            smokeRenderer.enabled = true;
        }


        respawnTimer = 5;
    }
}