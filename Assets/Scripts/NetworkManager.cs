using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playButtonMonkey;
    public GameObject playButtonPlane;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        playButtonMonkey.SetActive(true);
        playButtonPlane.SetActive(true);
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinTeam(int team)
    {
        //do we already have a team?
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", team }
        };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }

        //join the random room and launch game- the GameManager will spawn the correct model in based on the property
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a room and failed");
        int randomRoomName = Random.Range(0, 10000);
        PhotonNetwork.CreateRoom("Room" + randomRoomName, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        int randomRoomName = Random.Range(0, 10000);
        PhotonNetwork.CreateRoom("Room" + randomRoomName, new RoomOptions { MaxPlayers = 2 });
    }

    public void OnPlayerClickMonkey()
    {
        Debug.Log("Monkey button clicked");
        playButtonMonkey.SetActive(false);
        playButtonPlane.SetActive(false);
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPlayerClickPlane()
    {
        Debug.Log("Plane button clicked");
        playButtonMonkey.SetActive(false);
        playButtonPlane.SetActive(false);
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

}
