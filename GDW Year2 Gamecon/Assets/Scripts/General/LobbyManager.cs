using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Alteruna;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Services.Authentication.PlayerAccounts;
using NUnit.Framework.Internal;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager Instance { get; private set; }


    public string joinCodeLobby;
    [SerializeField] private RelayManager relayManager;
    [SerializeField] private GameObject LobbyCreationParent;
    [SerializeField] private GameObject LobbyListParent;

    public Transform lobbyContentParent;
    public GameObject LobbyItemPrefab;
    private bool _isPolling;

    private void Awake()
    {
        Instance = this;
    }




    private async void Start()
    {

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (LobbyListParent.activeInHierarchy)
            ShowLobbies();

    }


    public async void JoinLobby(string lobbyID, bool needPassword)
    {

        if (needPassword)
        {
            try
            {

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            try
            {
                await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);
                joinCodeLobby = lobbyID;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }







    private async void ShowLobbies()
    {
        if (_isPolling) return;
        _isPolling = true;

        while (Application.isPlaying)
        {
            // Destroy BEFORE querying
            foreach (Transform t in lobbyContentParent)
                Destroy(t.gameObject);

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            int ind = 1;
            foreach (Lobby lobby in queryResponse.Results)
            {
                Vector3 pos = new Vector3(
                    lobbyContentParent.position.x,
                    lobbyContentParent.position.y - (ind * 120),
                    lobbyContentParent.position.z
                );


                GameObject newLobbyItem = Instantiate(LobbyItemPrefab, lobbyContentParent);
                newLobbyItem.transform.position = pos;
                newLobbyItem.GetComponent<LobbyButtons>().lobbyID = lobby.Id;
                newLobbyItem.GetComponent<LobbyButtons>().needPassword = lobby.HasPassword;
                ind++;
            }

            await Task.Delay(3000); // 1s is aggressive — 3s is safer for rate limits
        }

        _isPolling = false;
    }


    public void ExitLobbyCreationButton()
    {
        LobbyListParent.SetActive(true);
        if (!_isPolling)
            ShowLobbies();
    }




    public async void CreateLobby()
    {

        Lobby createdLobby = null;

        try
        {
            var options = new CreateLobbyOptions { IsPrivate = false };
            createdLobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", 4, options);
            joinCodeLobby = createdLobby.Id;
            Debug.Log(joinCodeLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

        LobbyHeartBeat(createdLobby);
        ExitLobbyCreationButton();
    }


    private async void LobbyHeartBeat(Lobby lobby)
    {
        while (true)
        {
            if (lobby == null)
            {
                return;
            }

            await LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);

            await Task.Delay(15 * 1000);
        }
    }



}
