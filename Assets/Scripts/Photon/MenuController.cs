//using System.Collections;
//using System.Collections.Generic;
//using Photon.Pun;
//using UnityEngine;

//public class MenuController : MonoBehaviour
//{
//    public void OnClickCharacterPick()
//    {
//        if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonRoom.room.photonPlayers[0].ActorNumber)
//        {
//            if (PlayerInfo.PI != null)
//            {
//                PlayerInfo.PI.mySelectedCharacter = 0;
//                PlayerPrefs.SetInt("MyCharacter", 0);
//            }
//        }
//        else
//        {
//            if (PlayerInfo.PI != null)
//            {
//                PlayerInfo.PI.mySelectedCharacter = 1;
//                PlayerPrefs.SetInt("MyCharacter", 1);
//            }
//        }
//    }
//}
