using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    public static Lobby lobby;

    public GameObject playButton2;
    public GameObject playButton3;
    public GameObject playButton4;

    public int numPlayers;

    //public GameObject cancelButton;

    private void Awake()
    {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connects to Master photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton2.SetActive(true);
        playButton3.SetActive(true);
        playButton4.SetActive(true);
    }

    //public void On1PlayerClick()
    //{
    //    Debug.Log("Play button clicked (1)");
    //    numPlayers = 1;
    //    playButton.SetActive(false);
    //    //cancelButton.SetActive(true);
    //    PhotonNetwork.JoinRandomRoom();
    //}

    public void On2PlayerClick()
    {
        Debug.Log("Play button clicked (2)");
        numPlayers = 2;
        playButton2.SetActive(false);
        playButton3.SetActive(false);
        playButton4.SetActive(false);
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void On3PlayerClick()
    {
        numPlayers = 3;
        Debug.Log("Play button clicked (3)");
        playButton2.SetActive(false);
        playButton3.SetActive(false);
        playButton4.SetActive(false);        
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void On4PlayerClick()
    {
        Debug.Log("Play button clicked (4)");
        numPlayers = 4;
        playButton2.SetActive(false);
        playButton3.SetActive(false);
        playButton4.SetActive(false);
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random room but failed. There must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)numPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    //public void OnCancelButtonClicked()
    //{
    //    cancelButton.SetActive(false);
    //    playButton.SetActive(true);
    //    PhotonNetwork.LeaveRoom();
    //}

}
