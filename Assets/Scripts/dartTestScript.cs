using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartTestScript : MonoBehaviour
{
    public GameObject projectile;
    int gametic = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gametic % 5 == 0)
        {
            spawnDart();
        }
        gametic++;
    }

    void spawnDart()
    {
        GameObject newProjectile = Instantiate(projectile, this.transform.position + this.transform.transform.forward * 1.5f, this.transform.rotation);
        //Give it purpose
        newProjectile.GetComponent<Rigidbody>().AddForce(this.transform.forward * 40, ForceMode.Impulse);
        newProjectile.GetComponent<ProjectileScript>().setImpactDamageOfWeapon(1);
    }
}
