using System.Collections.Generic;
using UnityEngine;

public class CardManagement : MonoBehaviour
{
    private List<string> _currentCards;
    private string _selectedCard;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentCards = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(string cardName)
    {
        if (_currentCards.Count < 3)
        {
            _currentCards.Add(cardName);
        }
    }
    
    public string GetSelectedCard()
    {
        return _selectedCard;
    }
}
