using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject cardPrefab;
    
    [SerializeField]
    public Transform handPanel;
    
    [SerializeField]
    public Transform filedPanel;
    
    public void Start()
    {
        cardPrefab.AddComponent<CardDraggable>().fieldPanel = filedPanel;
        for (var i = 0; i < 7; i++)
        {
            Instantiate(cardPrefab, handPanel);
        }
    }
}
