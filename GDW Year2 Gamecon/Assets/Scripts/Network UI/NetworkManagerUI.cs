using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : TestRelay
{
    //[SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        // serverBtn.onClick.AddListener((() =>
        // {
        //     
        //     //NetworkManager.Singleton.StartServer();
        // }));

        clientBtn.onClick.AddListener((() =>
                {
                    JoinRelay(inputField.text);
                    //NetworkManager.Singleton.StartClient();
                }
            ));
        hostBtn.onClick.AddListener((() =>
                {
                    CreateRelay();
                 //   NetworkManager.Singleton.StartHost();
                }
            ));
    }
}
