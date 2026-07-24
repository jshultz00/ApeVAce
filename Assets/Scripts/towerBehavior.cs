using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerBehavior : MonoBehaviour
{
    public GameObject ledge;

    private float yMin;
    private float yMax;
    private float zMin;
    private float zMax;

    private float ledgeWidth;
    private int numLedges = 200;

    //Lists for each side to halve (O^2)
    //Might be a better algorithm so we avoid this, but works for now
    //This is only called at start as well, so no effect during gameplay.
    private List<Vector3> spawnPositionsX = new List<Vector3>();
    private List<Vector3> spawnPositionsZ = new List<Vector3>();
    private List<Vector3> spawnPositionsnX = new List<Vector3>();
    private List<Vector3> spawnPositionsnZ = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        var ledgeDisplace = this.transform.localScale.x / 2 + 0.01f;
        yMin = this.transform.position.y - this.transform.localScale.y / 2;
        yMax = this.transform.position.y + this.transform.localScale.y / 2;
        zMin = this.transform.position.z - this.transform.localScale.z / 2;
        zMax = this.transform.position.z + this.transform.localScale.z / 2;
        ledgeWidth = ledge.transform.localScale.z;

        //incredible copy/pasting but with slight sign/positional changes to randomly generate all positions
        for (int i = 0; i < numLedges; i++)
        {
            var ydisplace = (Random.Range(yMin, yMax));
            var zdisplace = (Random.Range(-(zMax-zMin-ledgeWidth)/2, (zMax - zMin - ledgeWidth) / 2));
            var spawnPos = new Vector3(ledgeDisplace, ydisplace - (yMax - yMin) / 2, zdisplace);
            spawnPos += transform.position;
            if (checkPosition(spawnPos, spawnPositionsX))
            {
                spawnPositionsX.Add(spawnPos);
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i < numLedges; i++)
        {
            var ydisplace = (Random.Range(yMin, yMax));
            var zdisplace = (Random.Range(-(zMax - zMin - ledgeWidth) / 2, (zMax - zMin - ledgeWidth) / 2));
            var spawnPos = new Vector3(-ledgeDisplace, ydisplace - (yMax - yMin) / 2, zdisplace);
            spawnPos += transform.position;
            if (checkPosition(spawnPos, spawnPositionsnX))
            {
                spawnPositionsnX.Add(spawnPos);
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i < numLedges; i++)
        {
            var ydisplace = (Random.Range(yMin, yMax));
            var zdisplace = (Random.Range(-(zMax - zMin - ledgeWidth) / 2, (zMax - zMin - ledgeWidth) / 2));
            var spawnPos = new Vector3(zdisplace, ydisplace - (yMax - yMin) / 2, ledgeDisplace);
            spawnPos += transform.position;
            if (checkPosition(spawnPos, spawnPositionsZ))
            {
                spawnPositionsZ.Add(spawnPos);
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i < numLedges; i++)
        {
            var ydisplace = (Random.Range(yMin, yMax));
            var zdisplace = (Random.Range(-(zMax - zMin - ledgeWidth) / 2, (zMax - zMin - ledgeWidth) / 2));
            var spawnPos = new Vector3(zdisplace, ydisplace - (yMax - yMin) / 2, -ledgeDisplace);
            spawnPos += transform.position;
            if (checkPosition(spawnPos, spawnPositionsnZ))
            {
                spawnPositionsnZ.Add(spawnPos);
            }
            else
            {
                i--;
            }
        }

        //To get correct forwards
        var rotation90 = Quaternion.identity * Quaternion.Euler(0, 90, 0);
        var rotation180 = Quaternion.identity * Quaternion.Euler(0, 180, 0);
        var rotation270 = Quaternion.identity * Quaternion.Euler(0, 270, 0);

        //Actually spawn in from our list positions
        foreach (Vector3 pos in spawnPositionsX)
        {
            GameObject newLedge = Instantiate(ledge, pos, Quaternion.identity);
            newLedge.transform.parent = this.transform;
        }
        foreach (Vector3 pos in spawnPositionsnX)
        {
            GameObject newLedge = Instantiate(ledge, pos, rotation180);
            newLedge.transform.parent = this.transform;
        }

        foreach (Vector3 pos in spawnPositionsZ)
        {
            GameObject newLedge = Instantiate(ledge, pos, rotation270);
            newLedge.transform.parent = this.transform;
        }
        foreach (Vector3 pos in spawnPositionsnZ)
        {
            GameObject newLedge = Instantiate(ledge, pos, rotation90);
            newLedge.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //We're mainly concerned with the y positions so that ledges don't overlap.
    bool checkPosition(Vector3 newPos, List<Vector3> list)
    {
        foreach (Vector3 existingPos in list)
        {
            if (Mathf.Abs(newPos.y - existingPos.y) < (ledge.transform.localScale.y * 1.5f))
            {
                return false;
            }
        }
        return true;
    }
}