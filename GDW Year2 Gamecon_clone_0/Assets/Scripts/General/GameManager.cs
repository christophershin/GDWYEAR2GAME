using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    public List<GameObject> spawnList;
    public Transform playerPrefab;


    public override void OnNetworkSpawn()
    {

        SpawnPlayerObjectServerRPC(NetworkManager.LocalClientId);
        

    }



    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerObjectServerRPC(ulong joinedClientId)
    {



        int SpawnCount = spawnList.Count;

        int randNum = UnityEngine.Random.Range(0, SpawnCount);

        float spawnX = spawnList[randNum].transform.position.x;
        float spawnY = spawnList[randNum].transform.position.y;
        float spawnZ = spawnList[randNum].transform.position.z;

        Vector3 playSpawn = new Vector3(spawnX, spawnY, spawnZ);


        Transform newGameObject = Instantiate(playerPrefab, playSpawn, Quaternion.identity);
        NetworkObject newNetworkObject = newGameObject.GetComponent<NetworkObject>();
        newNetworkObject.SpawnAsPlayerObject(joinedClientId, true);
    }




}
