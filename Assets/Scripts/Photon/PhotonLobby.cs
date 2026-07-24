using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject monkeyButton;
    public GameObject planeButton;
    public GameObject cancelButton;
    public GameObject offline;

    private void Awake()
    {
        lobby = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        monkeyButton.SetActive(true);
        planeButton.SetActive(true);
        offline.SetActive(false);
    }

    public void OnPlayButtonClicked(int whichCharacter)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
        Debug.Log("Play button clicked");
        monkeyButton.SetActive(false);
        planeButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new oom but failed, there must already be a room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button clicked");
        cancelButton.SetActive(false);
        monkeyButton.SetActive(true);
        planeButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(0);
        }
        PhotonNetwork.LeaveRoom();
    }
}
