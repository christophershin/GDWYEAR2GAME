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

    //public RectTransform[] rts;
    
    // Card Positions
    //[SerializeField] private GameObject[] slots; // 0: front, 1: middle, 2: back

    [SerializeField] private Image[] ogImages;
    [SerializeField] private RectTransform[] positions; // reference for the positions
    [SerializeField] private List<Image> movableImages; // Images
    [SerializeField] private List<RectTransform> _movables; // the actual cards to move
    
    // Sprites
    [SerializeField] private Sprite puck, grenade, pikeball, knife;
    
    // temporary for testing
    public void adCrd(string card)
    {
        AddCard(card);
    }

    private void Update()
    {
        // Card inputs
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0f)
            (scroll > 0f ? (System.Action)RotateFirstToLast : RotateLastToFirst)();
        
        for (int i = 0; i < _movables.Count; i++)
        {
            _movables[i].anchoredPosition = Vector2.Lerp(_movables[i].anchoredPosition, positions[i].anchoredPosition, Time.deltaTime * 6f);
            _movables[i].localRotation = Quaternion.Lerp(_movables[i].localRotation, positions[i].localRotation, Time.deltaTime * 6f);
        }
    }

    // Add Card
    public bool AddCard(string card)
    {
        if (_cards.Count >= 3) return false;
        _cards.Add(card);
        
        var currentCard = _cards.Count - 1;
        
        // This isn't a good way to do this, but I'll look for some better way later
        switch (card)
        {
            case "Puck":
                movableImages[currentCard].sprite = puck;
                break;
            case "Grenade":
                movableImages[currentCard].sprite = grenade;
                break;
            case "Pikeball":
                movableImages[currentCard].sprite = pikeball;
                break;
            case "Knife":
                movableImages[currentCard].sprite = knife;
                break;
        }
        
        movableImages[currentCard].color = ogImages[currentCard].color;
        
        // Activates / deacticvates slots
        for (int i = 0; i < movableImages.Count; i++)
        {
            movableImages[i].gameObject.SetActive(i < _cards.Count);
        }
        
        return true;
    }
    
    // Rotate cards Left
    public void RotateFirstToLast()
    {
        if (_cards.Count <= 1) return;
        
        if (_cards.Count == 2)
        {
            var temp = movableImages[0];
            movableImages.RemoveAt(0);
            movableImages.Insert(1, temp);
        }
        
        if (_cards.Count == 3)
        {
            var temp = movableImages[0];
            movableImages.RemoveAt(0);
            movableImages.Add(temp);
        }
        
        _movables.Clear();
        
        // Set the sibling index
        for (int i = 0; i < movableImages.Count; i++)
        {
            _movables.Add(movableImages[i].rectTransform);
            _movables[i].SetSiblingIndex((movableImages.Count - 1) - i);
            movableImages[i].color = ogImages[i].color;
        }
        
        var first = _cards[0];
        _cards.RemoveAt(0);  
        _cards.Add(first);
    }
    
    // Rotate cards right
    public void RotateLastToFirst()
    {
        if (_cards.Count <= 1) return;
        
        if (_cards.Count == 2)
        {
            var temp = movableImages[0];
            movableImages.RemoveAt(0);
            movableImages.Insert(1, temp);
        }
        
        if (_cards.Count == 3)
        {
            var temp = movableImages[2];
            movableImages.RemoveAt(2);
            movableImages.Insert(0, temp);
        }
        
        _movables.Clear();
        
        // Set the sibling index
        for (int i = 0; i < movableImages.Count; i++)
        {
            _movables.Add(movableImages[i].rectTransform);
            _movables[i].SetSiblingIndex((movableImages.Count - 1) - i);
            movableImages[i].color = ogImages[i].color;
        }
        
        var currentCard = _cards.Count - 1;
        
        var last = _cards[currentCard];
        _cards.RemoveAt(_cards.Count - 1);
        _cards.Insert(0, last);
    }
}