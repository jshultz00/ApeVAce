using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody rb;
    //Store sounds and visuals in impact effect
    public GameObject impactEffect;
    public LayerMask damageable;
    public LayerMask disappear;

    public int impactDamage;

    public float lifetime;
    PhysicMaterial physmat;

    //creates a physics material with given properties and applies it to the projectile.
    private void Setmat()
    {
        physmat = new PhysicMaterial();
        physmat.frictionCombine = PhysicMaterialCombine.Minimum;
        physmat.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<SphereCollider>().material = physmat;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //As it is right now, main hitbox is too large! Hand hitboxes will never come into play.
        Debug.Log("Contact: " + collision.transform.name);
        if (collision.gameObject.layer == Mathf.Log(disappear.value, 2))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == Mathf.Log(damageable.value, 2))
        {
            collision.collider.gameObject.GetComponent<MonkeyBehavior>().onHit(impactDamage);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Setmat();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime * 5;
        if (lifetime <= 0) Destroy(gameObject);
    }

    public void setImpactDamageOfWeapon(int setDamage)
    {
        impactDamage = setDamage;
    }
}