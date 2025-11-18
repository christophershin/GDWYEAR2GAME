using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    // Array
    private List<string> _cards = new List<string>(3);

    public RectTransform[] rts;
    
    // Card Positions
    [SerializeField] private GameObject[] slots; // 0: front, 1: middle, 2: back
    
    // Sprites
    [SerializeField] private Sprite puck, grenade, pikeball, knife;

    public void adCrd(string card)
    {
        AddCard(card);
    }

    private void Update()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            var childImage = slots[i].transform.GetChild(0).GetComponent<Image>().rectTransform;
            
            childImage.anchoredPosition = Vector2.Lerp(childImage.anchoredPosition, rts[i].anchoredPosition, Time.deltaTime * 10f);
            childImage.localRotation = Quaternion.Lerp(childImage.localRotation, rts[i].localRotation, Time.deltaTime * 10f);
        }
    }

    // Add Card
    public bool AddCard(string card)
    {
        if (_cards.Count >= 3) return false;
        _cards.Add(card);
        
        var currentCard = _cards.Count - 1;
        
        // get the child of the slot
        Transform child = slots[currentCard].transform.GetChild(0);
        child.transform.SetParent(slots[currentCard].transform, true);
        
        // This isn't a good way to do this, but I'll look for some better way later
        switch (card)
        {
            case "Puck":
                child.gameObject.GetComponent<Image>().sprite = puck;
                break;
            case "Grenade":
                child.gameObject.GetComponent<Image>().sprite = grenade;
                break;
            case "Pikeball":
                child.gameObject.GetComponent<Image>().sprite = pikeball;
                break;
            case "Knife":
                child.gameObject.GetComponent<Image>().sprite = knife;
                break;
        }
        
        // Activates / deacticvates slots
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(i < _cards.Count);
        }
        
        return true;
    }
    
    // Remove card
    public bool RemoveCard(string card)
    {
        return _cards.Remove(card);
    }
    
    // Rotate cards Left
    public void RotateFirstToLast()
    {
        if (_cards.Count <= 1) return;
        
        if (_cards.Count == 2)
        {
            Transform child1 = slots[0].transform.GetChild(0);
            child1.transform.SetParent(slots[1].transform, true);
                
            Transform child2 = slots[1].transform.GetChild(0);
            child2.transform.SetParent(slots[0].transform, true);
        }
        
        if (_cards.Count == 3)
        {
            Transform child1 = slots[0].transform.GetChild(0);
            child1.transform.SetParent(slots[2].transform, true);
                
            Transform child2 = slots[1].transform.GetChild(0);
            child2.transform.SetParent(slots[0].transform, true);
            
            Transform child3 = slots[2].transform.GetChild(0);
            child3.transform.SetParent(slots[1].transform, true);
        }
        
        var first = _cards[0];
        _cards.RemoveAt(0);  
        _cards.Add(first);
    }
    
    // Rotate cards right
    public void RotateLastToFirst()
    {
        if (_cards.Count <= 1) return;
        
        var currentCard = _cards.Count - 1;
        
        var last = _cards[currentCard];
        _cards.RemoveAt(_cards.Count - 1);
        _cards.Insert(0, last);
    }
}