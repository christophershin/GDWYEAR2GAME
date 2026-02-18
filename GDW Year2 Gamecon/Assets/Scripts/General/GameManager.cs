using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    
    public List<GameObject> spawnList;

    public Transform playerPrefab;

    [HideInInspector]
    public List<GameObject> allPlayers; 


    [HideInInspector]
    public NetworkList<ulong> PlayersInServer;

    private int numPlayerEliminated = 0;

    [SerializeField]
    private string menu;

    void Awake()
    {
        PlayersInServer = new NetworkList<ulong>();
    }


    public override void OnNetworkSpawn()
    {
        int SpawnCount = spawnList.Count;

        int randNum = UnityEngine.Random.Range(0, SpawnCount);

        float spawnX = spawnList[randNum].transform.position.x;
        float spawnY = spawnList[randNum].transform.position.y;
        float spawnZ = spawnList[randNum].transform.position.z;

        Vector3 playSpawn = new Vector3(spawnX, spawnY, spawnZ);


        SpawnPlayerObjectServerRPC(NetworkManager.LocalClientId, playSpawn);
        

    }


    private void Update()
    {
      
        if(PlayersInServer.Count>1)
        {
            for (int i = 0; i < PlayersInServer.Count; i++)
            {

                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(PlayersInServer[i], out NetworkObject netObj))
                {
                    GameObject obj = netObj.gameObject;


                    if (obj.GetComponent<HealthandShield>().Health.Value <= 0)
                    {
                        obj.GetComponent<HealthandShield>().CenterText.text = "defeat";
                        numPlayerEliminated++;

                    }
                    else if (numPlayerEliminated > 0 && numPlayerEliminated == PlayersInServer.Count - 1)
                    {
                        obj.GetComponent<HealthandShield>().CenterText.text = "Victory!";
                        NetworkManager.SceneManager.LoadScene(menu, LoadSceneMode.Single);
                        if (IsServer)
                        {
                            
                            ActivateCursorClientRPC();

                        }


                    }

                }  


            }
        }



    }








    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerObjectServerRPC(ulong joinedClientId, Vector3 playSpawn)
    {

        Transform newGameObject = Instantiate(playerPrefab, playSpawn, Quaternion.identity);
        NetworkObject newNetworkObject = newGameObject.GetComponent<NetworkObject>();
        newNetworkObject.SpawnAsPlayerObject(joinedClientId, true);

        if (IsServer)
        {
            ulong playerID = newGameObject.GetComponent<NetworkObject>().NetworkObjectId;
            PlayersInServer.Add(playerID);
        }


    }

    [ClientRpc]
    void ActivateCursorClientRPC()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


}
