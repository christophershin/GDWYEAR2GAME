using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class StartGameScript : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI startGameText;
    
    private bool _isSpawned = false;
    private GameObject[] _spawnArray;
    
    public static event Action ActivateCardsEvent;
    
    
    void Start()
    {
        if (IsServer)
        {
            startGameText.text = "Press G to start the game!";
        }
        else
        {
            startGameText.text = "Wait for the host to start the game!";
        }
        
        //_spawnArray = GameObject.FindGameObjectsWithTag("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer && _isSpawned == false)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                _isSpawned = true;
                TeleportClientRpc();
            }
        }
    }
    
    [ClientRpc]
    void TeleportClientRpc()
    {
        if (!IsOwner) return;
        StartCoroutine(StartTheGame());
    }
    
    private IEnumerator StartTheGame()
    {
        GameObject but = GameObject.FindGameObjectWithTag("NetworkButton");
        but.SetActive(false);
        
        startGameText.text = "Starting in 3";
        yield return new WaitForSeconds(1f);
        startGameText.text = "Starting in 2";
        yield return new WaitForSeconds(1f);
        startGameText.text = "Starting in 1";
        yield return new WaitForSeconds(1f);
        
        _spawnArray = GameObject.FindGameObjectsWithTag("Spawn");

        int randNum = UnityEngine.Random.Range(0, _spawnArray.Length);
        transform.position = _spawnArray[randNum].transform.position;

        ActivateCardsEvent?.Invoke();
        
        startGameText.text = "Good Luck :)";
        yield return new WaitForSeconds(2f);
        startGameText.text = "";
    }
}
