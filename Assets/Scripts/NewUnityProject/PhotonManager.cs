using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NewUnityProject
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] public string prefabName;
        [SerializeField] public Vector3 playerInitPosition;
        [SerializeField] public Vector3 playerInitRotation;

        public void Start()
        {
            Debug.Log("ログイン開始");
        
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
            PhotonNetwork.GameVersion = new Version(0, 1).ToString(); 
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("ログイン成功");
        
            var options = new RoomOptions()
            {
                MaxPlayers = 20,
                IsOpen = true,
                IsVisible = true,
            };
            PhotonNetwork.JoinOrCreateRoom("MainRoom", options, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("入室");
        
            PhotonNetwork.Instantiate(prefabName, playerInitPosition, Quaternion.Euler(playerInitRotation));
        }

        public override void OnLeftRoom()
        {
            Debug.Log("退室");
        }
    }
}