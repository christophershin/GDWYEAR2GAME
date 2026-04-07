using System.Collections;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CardSyncScript : NetworkBehaviour
{
    private NetworkVariable<FixedString32Bytes> _cardName = new NetworkVariable<FixedString32Bytes>();
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;
    
    
    [ServerRpc]
    public void ChangeCardNameServerRpc()
    {
        Debug.Log("Change Card Name server rpc called");
        _cardName.Value = "null";
        StartCoroutine(ResetCard());
    }
    
    public override void OnNetworkSpawn()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (IsServer)
        {
            _cardName.Value = GetRandomCardValue();
        }
        
        _cardName.OnValueChanged += ChangeCardName;
        ChangeCardName(new FixedString32Bytes(""), _cardName.Value); 
    }

    public override void OnNetworkDespawn()
    {
        _cardName.OnValueChanged -= ChangeCardName;
    }
    
    private void ChangeCardName(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        string nameString = newValue.ToString();
        this.transform.name = nameString;
        
        switch (nameString)
        {
            case "Puck":     _spriteRenderer.sprite = sprites[0]; break;
            case "Pikeball": _spriteRenderer.sprite = sprites[1]; break;
            case "Tomato":   _spriteRenderer.sprite = sprites[2]; break;
            case "Cone":     _spriteRenderer.sprite = sprites[3]; break;
            case "Speaker":  _spriteRenderer.sprite = sprites[4]; break;
            default: _spriteRenderer.sprite = null;  break;
        }
    }
    
    private string GetRandomCardValue() {
        int index = Random.Range(0, sprites.Length);
        return index switch {
            0 => "Puck",
            1 => "Pikeball",
            2 => "Tomato",
            3 => "Cone",
            4 => "Speaker",
            _ => "null"
        };
    }
    
    private IEnumerator ResetCard()
    {
        yield return new WaitForSeconds(7);
        _cardName.Value = GetRandomCardValue();
    }
}