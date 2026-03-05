using System.Collections.Generic;
using UnityEngine;

public class LookAtCards : MonoBehaviour
{
    GameObject[] _cards;
    private List<Transform> _transforms;
    void Start()
    {
        _cards = GameObject.FindGameObjectsWithTag("Card");

        // for (int i = 0; i < arrayOfCards.Length-1; i++)
        // {
        //     _transforms.Add(arrayOfCards[i].transform);
        // }
        
    }
    
    void Update()
    {

        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].transform.LookAt(transform.position);
        }
        
        // for (int i = 0; i < _transforms.Count; i++)
        // {
        //     _transforms[i].LookAt(transform.position);
        // }
        
    }
}
