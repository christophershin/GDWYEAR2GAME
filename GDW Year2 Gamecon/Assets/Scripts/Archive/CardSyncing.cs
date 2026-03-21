using Unity.Netcode;
using UnityEngine;

public class CardSyncing : NetworkBehaviour
{
    [SerializeField] private GameObject[] cards;
    [SerializeField] private Sprite[] cardPrefabs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        
    }

    public void CreateCards()
    {
        if (!IsServer) return;
        
        for (int i = 0; i < cards.Length; i++)
        {
            
            ulong objectId = cards[i].GetComponent<NetworkObject>().NetworkObjectId;
            int rand = Random.Range(0, cardPrefabs.Length);
            AddCardClientRPC(objectId, rand);
        }
    }
    
    [ClientRpc]
    public void AddCardClientRPC(ulong objectId, int rand)
    {
        
    }

    [ClientRpc]
    public void RemoveCardClientRPC(int i)
    {
        cards[i].gameObject.GetComponent<SpriteRenderer>().sprite = null;
        cards[i].gameObject.transform.name = "null";
    }
    
}
