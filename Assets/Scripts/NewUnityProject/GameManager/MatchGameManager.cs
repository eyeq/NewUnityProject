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
using UnityEngine.SceneManagement;

namespace NewUnityProject.GameManager
{
    public class MatchGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] public CardController cardPrefab;

        [SerializeField] public Transform playerDeckPanel;
        [SerializeField] public Transform playerFieldPanel;
        [SerializeField] public Transform playerHandPanel;

        [SerializeField] public Transform opponentDeckPanel;
        [SerializeField] public Transform opponentFieldPanel;
        [SerializeField] public Transform opponentHandPanel;

        /// <summary>
        /// Master
        /// </summary>
        private readonly List<MatchPlayerModel> _modelList = new List<MatchPlayerModel>();

        /// <summary>
        /// Client
        /// </summary>
        private readonly SemaphoreSlim _deckSemaphore = new SemaphoreSlim(1, 1);
        
        public void Start()
        {
            photonView.RPC(nameof(JoinGame), RpcTarget.MasterClient, new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13});
        }

        public void DrawCard()
        {
            photonView.RPC(nameof(Draw), RpcTarget.MasterClient);
        }

        /// <summary>
        /// master
        /// </summary>
        [PunRPC]
        public void JoinGame(int[] deck, PhotonMessageInfo info)
        {
            lock (_modelList)
            {
                if (_modelList.Any(d => d.PlayerId == info.Sender.UserId))
                {
                    Init(info.Sender);
                    return;
                }

                _modelList.Add(new MatchPlayerModel()
                {
                    Number = -1,
                    PlayerId = info.Sender.UserId,
                    Deck = deck.ToList(),
                    Field = new List<int>(),
                    Hand = new List<int>(),
                });

                if (_modelList.Count == 2)
                {
                    var newList = new List<MatchPlayerModel>();
                    
                    var number = 0;
                    foreach (var model in _modelList)
                    {
                        var temp = model;
                        temp.Number = number;
                        temp.Deck.Shuffle();
                        newList.Add(model);

                        number++;
                    }
                    _modelList.Clear();
                    _modelList.AddRange(newList);
                    
                    var playerList = PhotonNetwork.PlayerList;
                    foreach (var player in playerList)
                    {
                        Init(player);
                    }
                    
                    foreach (var model in _modelList)
                    {
                        Draw(model, 7);
                    }
                }
            }
        }

        /// <summary>
        /// master
        /// </summary>
        private void Init(Player player)
        {
            foreach (var model in _modelList)
            {
                if (model.PlayerId == player.UserId)
                {
                    photonView.RPC(nameof(UpdateGame), player,
                        model.PlayerId,
                        model.Deck.Select(d => -1).ToArray(),
                        model.Field.Select(d => d).ToArray(),
                        model.Hand.Select(d => d).ToArray());
                }
                else
                {
                    photonView.RPC(nameof(UpdateGame), player,
                        model.PlayerId,
                        model.Deck.Select(d => -1).ToArray(),
                        model.Field.Select(d => d).ToArray(),
                        model.Hand.Select(d => -1).ToArray());
                }
            }
        }
        
        /// <summary>
        /// master
        /// </summary>
        [PunRPC]
        public void Draw(PhotonMessageInfo info)
        {
            lock (_modelList)
            {
                var model = _modelList.First(d => d.PlayerId == info.Sender.UserId);
                Draw(model, 1);
            }
        }
        
        /// <summary>
        /// master
        /// </summary>
        private void Draw(MatchPlayerModel model, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (!model.Deck.Any())
                {
                    photonView.RPC(nameof(EndGame), RpcTarget.AllBuffered, model.PlayerId);
                    _modelList.Clear();
                    return;
                }

                var card = model.Deck.First();
                model.Deck.RemoveAt(0);
                model.Hand.Add(card);

                photonView.RPC(nameof(AnimationGame), RpcTarget.AllBuffered, model.PlayerId, "draw", "");
                var playerList = PhotonNetwork.PlayerList;
                foreach (var player in playerList)
                {
                    if (player.UserId == model.PlayerId)
                    {
                        photonView.RPC(nameof(UpdateGame), player,
                            model.PlayerId,
                            model.Deck.Select(d => -1).ToArray(),
                            model.Field.Select(d => d).ToArray(),
                            model.Hand.Select(d => d).ToArray());
                    }
                    else
                    {
                        photonView.RPC(nameof(UpdateGame), player,
                            model.PlayerId,
                            model.Deck.Select(d => -1).ToArray(),
                            model.Field.Select(d => d).ToArray(),
                            model.Hand.Select(d => -1).ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// client
        /// </summary>
        [PunRPC]
        public async Task AnimationGame(string who, string does, string what, PhotonMessageInfo info)
        {
            try
            {
                await _deckSemaphore.WaitAsync();

                Transform deckPanel;
                Transform fieldPanel;
                Transform handPanel;
                if (photonView.Owner.UserId == who)
                {
                    deckPanel = playerDeckPanel;
                    fieldPanel = playerFieldPanel;
                    handPanel = playerHandPanel;
                }
                else
                {
                    deckPanel = opponentDeckPanel;
                    fieldPanel = opponentFieldPanel;
                    handPanel = opponentHandPanel;
                }

                if (does == "draw")
                {
                    var card = deckPanel.GetComponentsInChildren<CardController>().Last();
                    await AnimationCardMove(card.transform, handPanel, 1000);
                }
            }
            finally
            {
                _deckSemaphore.Release();
            }
        }

        /// <summary>
        /// client
        /// </summary>
        private async Task AnimationCardMove(Transform card, Transform target, float speed)
        {
            Debug.Log("ドロー");
                
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

        /// <summary>
        /// client
        /// </summary>
        [PunRPC]
        public async Task UpdateGame(string playerId, int[] deck, int[] field, int[] hand, PhotonMessageInfo info)
        {
            Debug.Log("画面更新");
            
            try
            {
                await _deckSemaphore.WaitAsync();
                
                Transform deckPanel;
                Transform fieldPanel;
                Transform handPanel;
                if (photonView.Owner.UserId == playerId)
                {
                    deckPanel = playerDeckPanel;
                    fieldPanel = playerFieldPanel;
                    handPanel = playerHandPanel;
                }
                else
                {
                    deckPanel = opponentDeckPanel;
                    fieldPanel = opponentFieldPanel;
                    handPanel = opponentHandPanel;
                }

                foreach (Transform child in deckPanel)
                {
                    Destroy(child.gameObject);
                }
                for (var i = 0; i < deck.Length; i++)
                {
                    var card = Instantiate(cardPrefab, deckPanel);
                    card.Init(new CardModel
                    {
                        Num = deck[i],
                    }, handPanel, fieldPanel);
                    if (deckPanel == playerDeckPanel)
                    {
                        card.transform.position = new Vector3(100 - i, 100, 0);
                    }
                    else
                    {
                        card.transform.position = new Vector3(860 + i, 440, 0);
                    }
                }

                foreach (Transform child in fieldPanel)
                {
                    Destroy(child.gameObject);
                }
                foreach (var t in field)
                {
                    var card = Instantiate(cardPrefab, fieldPanel);
                    card.Init(new CardModel
                    {
                        Num = t,
                    }, handPanel, fieldPanel);
                }

                var prevSort = new List<int>();
                for (var i = 0; i < handPanel.childCount; i++)
                {
                    var child = handPanel.GetChild(i).GetComponent<CardController>();
                    prevSort.Add(child.Model.Num);
                }

                foreach (Transform child in handPanel)
                {
                    Destroy(child.gameObject);
                }
                var handList = hand.ToList();
                foreach (var sort in prevSort)
                {
                    if (handList.Any(d => d == sort))
                    {
                        handList.Remove(handList.First(d => d == sort));

                        var card = Instantiate(cardPrefab, handPanel);
                        card.Init(new CardModel
                        {
                            Num = sort,
                        }, handPanel, fieldPanel);
                    }
                }

                foreach (var t in handList)
                {
                    var card = Instantiate(cardPrefab, handPanel);
                    card.Init(new CardModel
                    {
                        Num = t,
                    }, handPanel, fieldPanel);
                }
            }
            finally
            {
                _deckSemaphore.Release();
            }
        }

        /// <summary>
        /// client
        /// </summary>
        [PunRPC]
        public void EndGame(string losePlayerId, PhotonMessageInfo info)
        {
            SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
            PhotonNetwork.LeaveRoom();
        }
    }
}