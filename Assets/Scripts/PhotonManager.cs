using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public string prefabName;
    [SerializeField] public Vector3 playerInitPosition;
    [SerializeField] public Vector3 playerInitRotation;
    
    [SerializeField] public Transform canvas;

    public void Start()
    {
        Debug.Log("ログイン開始");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ログイン成功");
        PhotonNetwork.JoinOrCreateRoom("MainRoom", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("入室成功");
        var playerPrefab = PhotonNetwork.Instantiate(prefabName, playerInitPosition, Quaternion.Euler(playerInitRotation));
        playerPrefab.transform.SetParent(canvas, false);
    }
}