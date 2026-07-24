using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    GameObject[] monkeySpawns;
    GameObject[] planeSpawns;

    void Awake()
    {
        instance = this;
        monkeySpawns = GameObject.FindGameObjectsWithTag("Monkey");
        planeSpawns = GameObject.FindGameObjectsWithTag("Plane");
    }
    // Start is called before the first frame update
    public Transform GetRandomMonkeySpawn()
    {
        //return a transform for one of the red spawns
        return monkeySpawns[Random.Range(0, monkeySpawns.Length)].transform;
    }

    public Transform GetRandomPlaneSpawn()
    {
        //return a transform for one of the blue spawns
        return planeSpawns[Random.Range(0, planeSpawns.Length)].transform;
    }
    //this method gets given the team number to find a spawn for
    [PunRPC]
    public Transform GetTeamSpawn(int teamNumber)
    {
        return teamNumber == 0 ? GetRandomPlaneSpawn() : GetRandomMonkeySpawn();
    }
}
