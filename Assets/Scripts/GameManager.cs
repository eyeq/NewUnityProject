using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject cardPrefab;
    
    [SerializeField]
    public Transform hand;
    
    public void Start()
    {
        for (var i = 0; i < 7; i++)
        {
            Instantiate(cardPrefab, hand);
        }
    }
    
    public void Update()
    {
        
    }
}
