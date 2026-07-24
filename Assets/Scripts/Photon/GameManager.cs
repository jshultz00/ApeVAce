using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject monkeyPrefab;
    public GameObject planePrefab;

    private void Start()
    {

        //check that we dont have a local instance before we instantiate the prefab
        if (NetworkPlayerManager.localPlayerInstance == null)
        {
            //instantiate the correct player based on the team
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log($"Team number {team} is being instantiated");
            //instantiate the blue player if team is 0 and red if it is not
            if (team == 0)
            {
                //get a spawn for the correct team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(0);
                PhotonNetwork.Instantiate(planePrefab.name, spawn.position, spawn.rotation);
            }
            else
            {
                //now for the red team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(1);
                PhotonNetwork.Instantiate(monkeyPrefab.name, spawn.position, spawn.rotation);
            }
        }

    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    } 

    //reload the level when anyone leaves or joins?- That is done in the demo but is it needed?
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ReloadLevel();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Reloading Level");
            PhotonNetwork.LoadLevel(1);
        }
    }


}