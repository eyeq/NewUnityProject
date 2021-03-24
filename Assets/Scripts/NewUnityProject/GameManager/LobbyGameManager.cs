using System;
using System.Linq;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NewUnityProject.GameManager
{
    public class LobbyGameManager : MonoBehaviourPunCallbacks
    {
        public const string RoomTypeKey = "RoomTypes";

        public static readonly string[] CustomRoomPropertiesForLobby =
        {
            RoomTypeKey,
        };

        public enum RoomType
        {
            Main = 1,
            QuickMatch = 2,
        }

        [SerializeField] public Button startButton;
        
        [SerializeField] public string playerPrefabName;
        [SerializeField] public Vector3 playerInitPosition;
        [SerializeField] public Vector3 playerInitRotation;

        private RoomType _roomType = RoomType.Main;
        
        public void Start()
        {
            Login();
        }

        public void StartMatch()
        {
            startButton.interactable = false;
            
            _roomType = RoomType.QuickMatch;
            PhotonNetwork.LeaveRoom();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("ログイン成功");

            if (_roomType == RoomType.Main)
            {
                // ロビーだとチャットもできないため、一旦MainRoomへ移動
                var expected = new Hashtable()
                {
                    {RoomTypeKey, (int) RoomType.Main}
                };
                var options = new RoomOptions()
                {
                    IsVisible = true,
                    IsOpen = true,
                    MaxPlayers = 20,
                    CustomRoomProperties = expected,
                    CustomRoomPropertiesForLobby = CustomRoomPropertiesForLobby,
                };
                PhotonNetwork.JoinOrCreateRoom("MainRoom", options, TypedLobby.Default);
            }
            else if (_roomType == RoomType.QuickMatch)
            {
                // StartMatch
                var expected = new Hashtable()
                {
                    {RoomTypeKey, (int) RoomType.QuickMatch}
                };
                PhotonNetwork.JoinRandomRoom(expected, 2);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("入室");
            
            if (PhotonNetwork.CurrentRoom?.Name == "MainRoom")
            {
                // MainRoom
                startButton.interactable = true;
                PhotonNetwork.Instantiate(playerPrefabName, playerInitPosition, Quaternion.Euler(playerInitRotation));
            }
            else
            {
                // StartMatch
                SceneManager.LoadScene("MatchScene", LoadSceneMode.Single);
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // StartMatch
            var expected = new Hashtable()
            {
                {RoomTypeKey, (int) RoomType.QuickMatch}
            };
            var options = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2,
                CustomRoomProperties = expected,
                CustomRoomPropertiesForLobby = CustomRoomPropertiesForLobby,
            };
            PhotonNetwork.CreateRoom(null, options);
        }

        private static void Login()
        {
            Debug.Log("ログイン開始");
	        
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
            PhotonNetwork.GameVersion = new Version(0, 1).ToString(); 
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}