using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MonkeyBehavior : MonoBehaviour
{
    public GameObject tower;
    public GameObject UICanvas;
    public GameObject monkSprite;
    public GameObject HPSprite;
    public GameObject lController;
    public GameObject rController;
    public GameObject monkeyCamera;
    public CharacterController curOrigin;

    private float maxHeight;
    private float minHeight;
    private float spriteMin = -0.45f;
    private float spriteMax = 0.45f;
    private Vector3 spritePos;
    private float heightPercentage;

    //Edit as needed
    private int maxHealth = 1000;
    private int health;
    private float healthPercentage;

    IXRInteractor inactiveController;
    IXRInteractor curController;

    //Vectors to determine velocity
    Vector3 prevPos = Vector3.zero;
    Vector3 curPos = Vector3.zero;

    Vector3 velocity = Vector3.zero;
    Vector3 heldVelocity;

    private bool gameOver = false;
    //Velocity multiplier so that we travel at a reasonable speed.
    float monkeSpeed = 70f;
    private bool ledgeHeld = false;

    //Jank soln so we don't reset. Half asleep so this is the way
    private int handCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        tower = GameObject.Find("tower");
        minHeight = tower.transform.localPosition.y - tower.transform.localScale.y / 2;
        maxHeight = tower.transform.localPosition.y + tower.transform.localScale.y / 2;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            checkForVictory();
            updateUI();
            Move();
        }
    }

    //Move the monkey
    private void Move()
    {
        if (ledgeHeld)
        {
            updateVelocityDirection();
            velocity = monkeSpeed * (prevPos - curPos);
            curOrigin.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (velocity.magnitude < heldVelocity.magnitude)
            {
                velocity = Vector3.MoveTowards(velocity, -heldVelocity, 5f * Time.deltaTime);
                curOrigin.Move(velocity * Time.deltaTime);
            }
        }
    }

    //Calculate velocity
    private void updateVelocityDirection()
    {
        if (prevPos == Vector3.zero)
        {
            prevPos = curController.transform.localPosition;
            curPos = curController.transform.localPosition;
        }
        prevPos = curPos;
        curPos = curController.transform.localPosition;
    }

    //Function that gets called by ledges when grabbed. Gives context information.
    public void ledgeGrabbed(IXRInteractor controller)
    {
        handCount++;
        if (handCount == 2)
        {
            prevPos = Vector3.zero;
            curPos = Vector3.zero;
            inactiveController = curController;
        }
        curController = controller;
        ledgeHeld = true;
    }

    public void releaseReset(IXRInteractor controller)
    {
        handCount--;
        prevPos = Vector3.zero;
        curPos = Vector3.zero;
        if (handCount == 1)
        {
            if (controller != inactiveController)
            {
                curController = inactiveController;
            }
            inactiveController = null;
        }
        if (handCount == 0)
        {
            if (velocity.magnitude > 3f)
            {
                velocity = velocity.normalized * 3f;
            }
            heldVelocity = velocity;
            velocity = Vector3.MoveTowards(velocity, -heldVelocity, 0.1f);
            curController = null;
            ledgeHeld = false;
        }
    }

    //Simple UI updating, could potentially move to a different script
    void updateUI()
    {
        heightPercentage = (this.transform.position.y) / (maxHeight - minHeight);
        spritePos = monkSprite.transform.localPosition;
        spritePos.y = heightPercentage * (spriteMax - spriteMin) + spriteMin;
        monkSprite.transform.localPosition = spritePos;
    }

    //Checks monkey victory condition.
    void checkForVictory()
    {
        //-1.5f for height difference
        if (this.transform.position.y >= (maxHeight - 1.5f))
        {
            gameOver = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(2); // Monkey win scene
            }
        }
    }

    //Checks plane team victory condition
    void checkForDeath()
    {
        if (health <= 0)
        {
            gameOver = true;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(3); // Planes win scene
            }
        }
    }

    //This function should be called by ProjectileScript. On hit with the monkey, causes damage, updates UI, and checks if plane team should win.
    //param damage: Passed from the projectile, how much damage it does. Useful if we end up having different weapon types.
    public void onHit(int damage)
    {
        if (!gameOver)
        {
            health -= damage;
            checkForDeath();
            updateHPVisual();
        }
    }

    void updateHPVisual()
    {
        healthPercentage = (float)health / (float)maxHealth;
        if (health < 0)
        {
            healthPercentage = 0f;
        }
        Vector3 spriteScale = HPSprite.transform.localScale;
        spriteScale.y = healthPercentage;
        HPSprite.transform.localScale = spriteScale;
    }
}