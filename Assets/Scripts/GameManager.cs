using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller;
using Event;
using Extensions;
using Model;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CardController cardPrefab;

    [SerializeField] public Transform deckPanel;
    
    [SerializeField] public Transform handPanel;

    [SerializeField] public Transform filedPanel;

    private readonly List<int> _deck = new List<int>();

    public void Start()
    {
        cardPrefab.gameObject.RemoveAllComponents<CardDraggable>();
        var cardDraggable = cardPrefab.gameObject.AddComponent<CardDraggable>();
        cardDraggable.handPanel = handPanel;
        cardDraggable.fieldPanel = filedPanel;

        _deck.Clear();
        _deck.AddRange(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13});
        _deck.Shuffle();

        for (var i = 0; i < _deck.Count; i++)
        {
            var card = Instantiate(cardPrefab, deckPanel);
            card.Init(new CardModel
            {
                Num = -1,
            });
            card.transform.position = new Vector3(100 - i, 100, 0);
        }

        Task.Run(async () =>
        {
            for (var i = 0; i < 7; i++)
            {
                await AsyncDrawCard();
            }
        });
    }

    public void DrawCard()
    {
        AsyncDrawCard();
    }
    
    private async Task AsyncDrawCard()
    {
        await Task.Run(async () =>
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
            });
            await CardMoveAnimation(card.transform, handPanel, 1000);
        });
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