using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;
    public GameObject myAvatar;
    public int myTeam;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myAvatar == null && myTeam != 0)
        {
            if (PlayerInfo.PI.mySelectedCharacter == 0)
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointMonkey.Length);
                if (PV.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Kong"),
                        GameSetup.GS.spawnPointMonkey[spawnPicker].position, GameSetup.GS.spawnPointMonkey[spawnPicker].rotation, 0);
                }
            }
            else
            {
                int spawnPicker = Random.Range(0, GameSetup.GS.spawnPointPlane.Length);
                if (PV.IsMine)
                {
                    myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "biplaneObject"),
                        GameSetup.GS.spawnPointPlane[spawnPicker].position, GameSetup.GS.spawnPointPlane[spawnPicker].rotation, 0);
                }
            }
        }
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = GameSetup.GS.nextPlayersTeam;
        GameSetup.GS.UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }
}
