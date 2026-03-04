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

public class RelayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private GameObject networkUI;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject cardsSpawner;
    
    // NETWORK UI
    [SerializeField] private GameObject networkBackground, hostButton, joinButton, allButton;



    public async void StartRelay()
    {

        Disconnect();
        string joinCode = await StartHostingWithRelay();
        //joinCodeText.text = joinCode;

    }

    public async void JoinRelay()
    {
        Disconnect();
        await StartClientWithRelay(joinCodeInputField.text);
    }

    public async Task<string> StartHostingWithRelay( int maxConnections = 3)
    {
        
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);


        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        
        
        //networkBackground.SetActive(false);
        //hostButton.SetActive(false);
        //joinButton.SetActive(false);
        
        ////cam.SetActive(false);
        //gameUI.SetActive(true);
        //gameplayUI.SetActive(true);
        //cardsSpawner.SetActive(true);
        
        
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        //gameplayUI.SetActive(true);
        
        //networkBackground.SetActive(false);
        //allButton.SetActive(false);
        
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }


    void Disconnect()
    {

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();


        }

    }


}


