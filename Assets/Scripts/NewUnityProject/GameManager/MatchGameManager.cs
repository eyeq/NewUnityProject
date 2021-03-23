using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewUnityProject.Controller;
using NewUnityProject.Extensions;
using NewUnityProject.Model;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NewUnityProject.GameManager
{
    public class MatchGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] public CardController cardPrefab;

        [SerializeField] public Transform deckPanel;

        [SerializeField] public Transform handPanel;

        [SerializeField] public Transform filedPanel;

	    [SerializeField] public string playerPrefabName;
	    [SerializeField] public Vector3 playerInitPosition;
	    [SerializeField] public Vector3 playerInitRotation;

        private readonly List<int> _deck = new List<int>();

        private readonly SemaphoreSlim _deckSemaphore = new SemaphoreSlim(1, 1);

#pragma warning disable 4014
        public void Start()
        {
	        Login();

            AsyncInitDeck(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13});

            AsyncDrawCard(7);
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
	        
	        PhotonNetwork.Instantiate(playerPrefabName, playerInitPosition, Quaternion.Euler(playerInitRotation));
	    }

	    public override void OnLeftRoom()
	    {
	        Debug.Log("退室");
	    }
	    
        public void DrawCard()
        {
            AsyncDrawCard(1);
        }
#pragma warning restore 4014

		private void Login()
		{
	        Debug.Log("ログイン開始");
	        
	        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
	        PhotonNetwork.GameVersion = new Version(0, 1).ToString(); 
	        PhotonNetwork.ConnectUsingSettings();
		}
		
        private async Task AsyncInitDeck(IEnumerable<int> deck)
        {
            Debug.Log("デッキ初期化");
            try
            {
                await _deckSemaphore.WaitAsync();

                _deck.Clear();
                _deck.AddRange(deck);
                _deck.Shuffle();

                for (var i = 0; i < _deck.Count; i++)
                {
                    var card = Instantiate(cardPrefab, deckPanel);
                    card.Init(new CardModel
                    {
                        Num = -1,
                    }, handPanel, filedPanel);
                    card.transform.position = new Vector3(100 - i, 100, 0);
                }
            }
            finally
            {
                _deckSemaphore.Release();
            }
        }

        private async Task AsyncDrawCard(int count)
        {
            Debug.Log($"ドロー: {count}");
            try
            {
                await _deckSemaphore.WaitAsync();

                for (var i = 0; i < count; i++)
                {
                    if (!_deck.Any())
                    {
                        return;
                    }

                    var cardEntity = _deck.First();
                    _deck.RemoveAt(0);

                    var card = deckPanel.GetComponentsInChildren<CardController>().Last();
                    card.Init(new CardModel
                    {
                        Num = cardEntity,
                    }, handPanel, filedPanel);
                    await CardMoveAnimation(card.transform, handPanel, 1000);
                }
            }
            finally
            {
                _deckSemaphore.Release();
            }
        }

        private async Task CardMoveAnimation(Transform card, Transform target, float speed)
        {
            transform.SetParent(card.parent.parent, false);
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;

            var vector = (target.position - card.position);
            vector *= (speed / vector.magnitude);
            while (0 < vector.x && card.position.x < target.position.x ||
                   0 > vector.x && card.position.x > target.position.x ||
                   0 < vector.y && card.position.y < target.position.y ||
                   0 > vector.y && card.position.y > target.position.y)
            {
                card.position += vector * Time.deltaTime;
                await Task.Delay(TimeSpan.FromSeconds(0.01f));
            }

            card.transform.SetParent(target, false);
            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}