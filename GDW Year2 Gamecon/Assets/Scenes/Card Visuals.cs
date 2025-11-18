using System;
using System.Collections.Generic;
using UnityEngine;

public class CardVisuals : MonoBehaviour
{
    // Cards manager script
    private CardsManager _cardsManager;
    
    

    private void Start()
    {
        _cardsManager = this.GetComponent<CardsManager>();
    }

    
}
