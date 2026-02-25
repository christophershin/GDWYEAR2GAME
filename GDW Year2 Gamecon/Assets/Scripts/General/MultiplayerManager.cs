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

public class MultiplayerManager : NetworkBehaviour
{


    [HideInInspector]
    public NetworkList<ulong> NetworkLobbyObjectList;
    public GameObject NetworkLobbyObject;
    public RectTransform NetworkSpawnPos;

    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private string NextScene;
    [SerializeField] private Transform HostRoom, Lobbyroom;


    void Awake()
    {
        NetworkLobbyObjectList = new NetworkList<ulong>();
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.StartHost();
    }


    //private async void Start()
    //{
    //    await UnityServices.InitializeAsync();
    //    await AuthenticationService.Instance.SignInAnonymouslyAsync();

    //}


    public void StartHosting()
    {
        CreateNetworkObject();
    }
    public void Play()
    {
        
        NetworkManager.SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
    }


    private async Task<string> StartHostingWithRelay(int maxConnections = 3)
    {

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);


        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    private void CreateNetworkObject()
    {
        GameObject netObj = Instantiate(NetworkLobbyObject, NetworkSpawnPos);

        ulong netId = netObj.GetComponent<NetworkObject>().NetworkObjectId;
        NetworkLobbyObjectList.Add(netId);

    }
}
