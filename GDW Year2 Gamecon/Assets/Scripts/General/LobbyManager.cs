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
using System.Collections;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager Instance { get; private set; }


    public string joinCodeLobby;
    [SerializeField] private RelayManager relayManager;
    [SerializeField] private GameObject LobbyCreationParent;
    [SerializeField] private GameObject LobbyListParent;
    [SerializeField] private GameObject LobbyJoinedScene;
    [SerializeField] private GameObject joinedLobbyStartButton;

    [SerializeField] private string Scene;
    [SerializeField] private NetworkManager networkManager;

    public Transform lobbyContentParent;
    public GameObject LobbyItemPrefab;
    private bool _isPolling;

    private string playerName;
    private Player playerData;
    private string joinedLobbyId;

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
            UpdateLobbyInfo();

        CreateProfile();
        
    }

    private void CreateProfile()
    {
        playerName = "bob";
        //profileSetupParent.SetActive(false);
        //lobbyListParent.SetActive(true);
        ShowLobbies();

        PlayerDataObject playerDataObjectName = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName);
        PlayerDataObject playerDataObjectTeam = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "A");

        playerData = new Player(id: AuthenticationService.Instance.PlayerId, data:
        new Dictionary<string, PlayerDataObject> { { "Name", playerDataObjectName }, { "Team", playerDataObjectTeam } });
    }


    public async void JoinLobby(string lobbyID, bool needPassword)
    {

        if (needPassword)
        {
            try
            {

                joinedLobbyId = lobbyID;
                UpdateLobbyInfo();
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
                joinedLobbyId = lobbyID;
                LobbyJoinedScene.SetActive(true);
                UpdateLobbyInfo();

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

            await Task.Delay(1000);
        }

        _isPolling = false;

    }


    public void ExitLobbyCreationButton()
    {
        LobbyJoinedScene.SetActive(true);
        if(!_isPolling)
            ShowLobbies();
    }




    public async void CreateLobby()
    {
        var options = new CreateLobbyOptions { IsPrivate = false };
        options.Player = playerData;
        Lobby createdLobby = null;


        DataObject dataObjectGameMode = new DataObject(DataObject.VisibilityOptions.Public, string.Empty);

        DataObject dataObjectJoinCode = new DataObject(DataObject.VisibilityOptions.Public, string.Empty);

        options.Data = new Dictionary<string, DataObject> { { "GameMode", dataObjectGameMode }, { "JoinCode", dataObjectJoinCode } };



        try
        {
            
            createdLobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", 4, options);
            joinedLobbyId = createdLobby.Id;

            UpdateLobbyInfo();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

        LobbyHeartBeat(createdLobby);
        ExitLobbyCreationButton();
    }



    private bool isJoined = false;
    private async void UpdateLobbyInfo()
    {
        while (Application.isPlaying)
        {
            if (string.IsNullOrEmpty(joinedLobbyId))
            {
                return;
            }

            Lobby lobby = await Lobbies.Instance.GetLobbyAsync(joinedLobbyId);

            if (!isJoined && lobby.Data["JoinCode"].Value != string.Empty)
            {
                await relayManager.StartClientWithRelay(lobby.Data["JoinCode"].Value);
                isJoined = true;
                Debug.Log("Client Connected!!!!");
                return;
            }

            if (AuthenticationService.Instance.PlayerId == lobby.HostId)
            {
                joinedLobbyStartButton.SetActive(true);
            }
            else
            {
                joinedLobbyStartButton.SetActive(false);
            }

            //joinedLobbyNameText.text = lobby.Name;
            //joinedLobbyGamemodeText.text = lobby.Data["GameMode"].Value;

            //foreach (Transform t in playerListParent)
            //{
            //    Destroy(t.gameObject);
            //}

            //foreach (Player player in lobby.Players)
            //{
            //    Transform newPlayerItem = Instantiate(playerItemPrefab, playerListParent);
            //    newPlayerItem.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.Data["Name"].Value;
            //    newPlayerItem.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.Data["Team"].Value;
            //    newPlayerItem.GetChild(2).GetComponent<TextMeshProUGUI>().text = (lobby.HostId == player.Id) ? "Owner" : "User";
            //}

            await Task.Delay(1000);
        }
    }




    public async void LobbyStart()
    {
        Lobby lobby = await Lobbies.Instance.GetLobbyAsync(joinedLobbyId);
        string JoinCode = await relayManager.StartHostingWithRelay(lobby.MaxPlayers);
        isJoined = true;
        await Lobbies.Instance.UpdateLobbyAsync(joinedLobbyId, new UpdateLobbyOptions
        { Data = new Dictionary<string, DataObject> { { "JoinCode", new DataObject(DataObject.VisibilityOptions.Public, JoinCode) } } });

        //lobbyListParent.SetActive(false);
        //joinedLobbyParent.SetActive(false);
        Debug.Log("Hosting!!!!");

        StartCoroutine(switchScene());
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

    private IEnumerator switchScene()
    {

        yield return new WaitForSeconds(1.5f);

        networkManager.SceneManager.LoadScene(Scene, LoadSceneMode.Single);

    }

}
