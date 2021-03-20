using System.Collections.Generic;
using System.Linq;
using Controller;
using Event;
using Extensions;
using Model;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CardController cardPrefab;

    [SerializeField] public Transform handPanel;

    [SerializeField] public Transform filedPanel;

    private readonly List<int> _deck = new List<int>();

    public void Start()
    {
        cardPrefab.gameObject.RemoveAllComponents<CardDraggable>();
        cardPrefab.gameObject.AddComponent<CardDraggable>().fieldPanel = filedPanel;

        _deck.Clear();
        _deck.AddRange(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13});
        _deck.Shuffle();

        for (var i = 0; i < 7; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (!_deck.Any())
        {
            return;
        }

        var cardEntity = _deck.First();

        var card = Instantiate(cardPrefab, handPanel);
        card.Init(new CardModel
        {
            Num = cardEntity,
        });

        _deck.RemoveAt(0);
    }
}