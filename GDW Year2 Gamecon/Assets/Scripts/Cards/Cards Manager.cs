using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : NetworkBehaviour
{
    [SerializeField] private GameObject puck, pikeball, tomato, cone, speaker;
    [SerializeField] private GameObject[] order; // the gameobject of the originals
    [SerializeField] private RectTransform[] orderRect; // the positions originals

    // Cards
    public List<GameObject> cardList = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    public void adcrd(string cardName)
    {
        AddCard(cardName);
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0f)
            (scroll > 0f ? (System.Action)FirstCardToLast : LastCardToFirst)();
        
        for (int i = 0; i < cardList.Count; i++)
        {
            // THIS IS AN EXPENSIVE METHOD CHANGE LATER
            var rct = cardList[i].GetComponent<Image>().GetComponent<RectTransform>();
            rct.anchoredPosition = Vector2.Lerp(rct.anchoredPosition, orderRect[i].anchoredPosition, Time.deltaTime * 6f);
            rct.localRotation = Quaternion.Lerp(rct.localRotation, orderRect[i].localRotation, Time.deltaTime * 6f);
        }
    }

    public bool AddCard(string nam)
    {
        //Debug.Log("Add card called: " + nam);

        if (cardList.Count >= 3)
        {
            //Debug.Log("Add returned false");
            return false;
        }
        
        // Instantiate the card
        switch (nam)
        {
            case "Puck":
                cardList.Add(Instantiate(puck, this.transform));
                break;
            case "Pikeball":
                cardList.Add(Instantiate(pikeball, this.transform));
                break;
            case "Tomato":
                cardList.Add(Instantiate(tomato,  this.transform));
                break;
            case "Cone":
                cardList.Add(Instantiate(cone,  this.transform));
                break;
            case "Speaker":
                cardList.Add(Instantiate(speaker,  this.transform));
                break;
            default:
                //Debug.Log("Add returned false");
                return false;
        }
        
        // Current Card
        var index = cardList.Count - 1;
        var currentCard = cardList[index];
        currentCard.SetActive(true);
        RectTransform rectTransform = currentCard.GetComponent<Image>().GetComponent<RectTransform>();
        
        // parent
        currentCard.transform.SetParent(order[index].transform, true);
        
        // name
        currentCard.name = nam;
        
        
        //Debug.Log("Add returned true");
        return true;
    }

    public string UseCard()
    {
        // nothing to remove
        if (cardList.Count == 0) return "";
        
        var card = cardList[0].name;
        
        // destroy the visual of the first card
        var firstCard = cardList[0];
        Destroy(firstCard);

        // remove from list
        cardList.RemoveAt(0);

        // shift remaining cards
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.SetParent(order[i].transform, true);
        }
        
        return card;
    }

    public void FirstCardToLast()
    {
        if (cardList.Count <= 1) return;
        
        var card = cardList[0];
        cardList.RemoveAt(0);
        cardList.Add(card);
        
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.SetParent(order[i].transform, true);
        }
    }
    
    public void LastCardToFirst()
    {
        if (cardList.Count <= 1) return;
        
        var index = cardList.Count - 1;
        var card = cardList[index];
        cardList.RemoveAt(index);
        cardList.Insert(0, card);
        
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.SetParent(order[i].transform, true);
        }
    }
}