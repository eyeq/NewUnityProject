using Controller;
using Model;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CardController cardPrefab;

    [SerializeField] public Transform handPanel;

    [SerializeField] public Transform filedPanel;

    public void Start()
    {
        cardPrefab.gameObject.AddComponent<CardDraggable>().fieldPanel = filedPanel;
        for (var i = 0; i < 7; i++)
        {
            var card = Instantiate(cardPrefab, handPanel);
            card.Init(new CardModel
            {
                Num = i,
            });
        }
    }
}