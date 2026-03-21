using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CardSpawner : NetworkBehaviour
{
    [SerializeField] private float waitTime;
    
    
    private List<GameObject> _cards = new List<GameObject>();
    private List<SpriteRenderer> _cardSprites = new List<SpriteRenderer>();
    [SerializeField] private Sprite[] cardSpritePrefabs;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        print("SERVER STARTED");
        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            
            _cards.Add(go);
            _cardSprites.Add(sprite);
            
            int rando =  Random.Range(0, 4);
            
            sprite.sprite = cardSpritePrefabs[rando];
            
            if (rando == 0)
            {
                go.name = "Puck";
            }
            else if (rando == 1)
            {
                go.name = "Grenade";
            }
            else if (rando == 2)
            {
                go.name = "Pikeball";
            }
            else if (rando == 3)
            {
                go.name = "Knife";
            }
        }
        
        
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsServer) return;
        
        for (int i = 0; i < _cards.Count; i++)
        {
            GameObject go = _cards[i];
            if (go.name == "null")
            {
                go.name = "awaiting";
                StartCoroutine(SpawnCard(i));
            }
        }
    }

    IEnumerator SpawnCard(int i)
    {
        yield return new WaitForSeconds(waitTime);
        
        int rando =  Random.Range(0, 4);
            
        _cardSprites[i].sprite = cardSpritePrefabs[rando];
            
        if (rando == 0)
        {
            _cards[i].name = "Puck";
        }
        else if (rando == 1)
        {
            _cards[i].name = "Grenade";
        }
        else if (rando == 2)
        {
            _cards[i].name = "Pikeball";
        }
        else if (rando == 3)
        {
            _cards[i].name = "Knife";
        }
    }
}
