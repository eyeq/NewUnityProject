using System;
using System.Collections.Generic;
using Extensions;
using Photon.Pun;
using UnityEngine;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public Vector2 guiScreenSize = new Vector2(960, 540);
    [SerializeField] public Vector2 guiSize = new Vector2(250, 300);
    [SerializeField] public Transform parent;
    [SerializeField] public bool isVisible = true;
    [SerializeField] public int maxCount = 10;
    
    private string _inputLine = "";
    private Vector2 _scrollPos = Vector2.zero;
    private readonly List<string> _messageList = new List<string>();

    public void OnGUI()
    {
        if (!isVisible || !PhotonNetwork.InRoom)
        {
            return;
        }

        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(_inputLine))
            {
                photonView.RPC(nameof(Chat), RpcTarget.All, _inputLine);
                _inputLine = "";
                GUI.FocusControl("");
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                GUI.FocusControl("ChatInput");
            }
        }
        
        // GUI用の解像度設定
        var scale = Math.Min(Screen.width  / guiScreenSize.x, Screen.height / guiScreenSize.y);
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
        
        GUI.SetNextControlName("");
        var guiRect = new Rect(parent.position.x / scale, (Screen.height - parent.position.y) / scale, guiSize.x, guiSize.y);
        GUILayout.BeginArea(guiRect);

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        GUILayout.FlexibleSpace();

        for (var i = 0; i < _messageList.Count; i++)
        {
            GUILayout.Label(_messageList[i]);

            if (_messageList.Count >= maxCount)
            {
                _messageList.RemoveAt(0);
            }
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
        _inputLine = GUILayout.TextField(_inputLine);
        if (GUILayout.Button("Send", GUILayout.ExpandWidth(false)))
        {
            if (!string.IsNullOrEmpty(_inputLine))
            {
                photonView.RPC(nameof(Chat), RpcTarget.All, _inputLine);
                _inputLine = "";
                GUI.FocusControl("");
            }
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndArea();
        
        // GUIの解像度を元に戻す
        GUI.matrix = Matrix4x4.identity; 
    }

    [PunRPC]
    public void Chat(string newLine, PhotonMessageInfo info)
    {
        var senderName = info.Sender.GetNicknameOrDefault();
        AddLine(senderName + ": " + newLine);
    }

    [PunRPC]
    public void Broadcast(string broadcast)
    {
        AddLine(broadcast);
    }

    public void AddLine(string newLine)
    {
        _messageList.Add(newLine);
    }
}