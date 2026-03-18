using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeleteCard : NetworkBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] cardSpritePrefabs;

    private int _rando;

    void UpdateCardClients()
    {
        if (!IsServer) return;
        UpdateCardClientRPC(_rando);
    }
    
    [ClientRpc]
    public void UpdateCardClientRPC(int rando)
    {
        //if (!IsOwner) return;
        
        if (rando == -1)
        {
            _spriteRenderer.sprite = null;
            gameObject.name = "null";
            return;
        }
        
        _spriteRenderer.sprite = cardSpritePrefabs[rando];
        
        switch (rando)
        {
            case 0:
                gameObject.name = "Puck";
                return;
            case 1:
                gameObject.name = "Grenade";
                return;
            case 2:
                gameObject.name = "Pikeball";
                return;
            case 3:
                gameObject.name = "Knife";
                return;
        }
    }

    private void Start()
    {
        _spriteRenderer =  GetComponent<SpriteRenderer>();
        StartGameScript.ActivateCardsEvent += UpdateCardClients;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        SpawnCardsServerRpc();
    }

    [ServerRpc]
    private void SpawnCardsServerRpc()
    {
        _rando =  Random.Range(0, 4);
        
        _spriteRenderer.sprite = cardSpritePrefabs[_rando];
        
        if (_rando == 0)
        {
            gameObject.name = "Puck";
        }
        else if (_rando == 1)
        {
            gameObject.name = "Grenade";
        }
        else if (_rando == 2)
        {
            gameObject.name = "Pikeball";
        }
        else if (_rando == 3)
        {
            gameObject.name = "Knife";
        }
        
        //UpdateCardClientRPC(_rando);
        //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRPC()
    {
        if (IsServer)
        {
            //NetworkObject.Despawn();

            _rando = -1;
            
            _spriteRenderer.sprite = null;
            gameObject.name = "null";
            
            UpdateCardClientRPC(_rando);
        }

        StartCoroutine(SpawnCard());
    }
    
    IEnumerator SpawnCard()
    {
        yield return new WaitForSeconds(10);
        
        _rando =  Random.Range(0, 4);
        
        _spriteRenderer.sprite = cardSpritePrefabs[_rando];
        
        if (_rando == 0)
        {
            gameObject.name = "Puck";
        }
        else if (_rando == 1)
        {
            gameObject.name = "Grenade";
        }
        else if (_rando == 2)
        {
            gameObject.name = "Pikeball";
        }
        else if (_rando == 3)
        {
            gameObject.name = "Knife";
        }
        
        UpdateCardClientRPC(_rando);
    }
    
    // public override void OnNetworkSpawn()
    // {
    //     if (!IsServer) return;
    //     
    //     _rando =  Random.Range(0, 4);
    //     
    //     _spriteRenderer.sprite = cardSpritePrefabs[_rando];
    //     
    //     if (_rando == 0)
    //     {
    //         gameObject.name = "Puck";
    //     }
    //     else if (_rando == 1)
    //     {
    //         gameObject.name = "Grenade";
    //     }
    //     else if (_rando == 2)
    //     {
    //         gameObject.name = "Pikeball";
    //     }
    //     else if (_rando == 3)
    //     {
    //         gameObject.name = "Knife";
    //     }
    //     
    //     UpdateCardClientRPC(_rando);
    //     
    //     //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    //     
    // }
    
    void OnClientConnected(ulong clientId)
    {
        UpdateCardClientRPC(_rando);
        
    }
}
