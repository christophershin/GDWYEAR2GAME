using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.UI;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    public List<GameObject> spawnList;

    public Transform playerPrefab;

    public List<GameObject> allPlayers; 


    [HideInInspector]
    public NetworkList<ulong> PlayersInServer;

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
        //Debug.Log(allPlayers.Count);
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




}
