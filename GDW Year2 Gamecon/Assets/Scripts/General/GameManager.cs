using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    public List<GameObject> spawnList;

    public Transform playerPrefab;

    [HideInInspector]
    public List<GameObject> allPlayers;
    
    //[SerializeField] private GameObject[] spawnArray;


    [HideInInspector]
    public NetworkList<ulong> PlayersInServer;

    private int numPlayerEliminated = 0;

    [SerializeField]
    private string menu;

    void Awake()
    {
        PlayersInServer = new NetworkList<ulong>();
    }
    
    // REMOVE BELOW IF BUG
    private void Start() {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId) {
        if (NetworkManager.Singleton.IsServer)
        {
            PlayersInServer.Remove(clientId);
        }
    }
    // REMOVE ABOVE IF BUG

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
        //if (!IsOwner) return;
        if (PlayersInServer.Count > 1)
        {

            numPlayerEliminated = 0;

            for (int i = 0; i < PlayersInServer.Count; i++)
            {

                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(PlayersInServer[i], out NetworkObject netObj))
                {
                    GameObject obj = netObj.gameObject;


                    if (obj.GetComponent<HealthandShield>().Health.Value <= 0)
                    {

                        if (!obj.CompareTag("Dead"))
                        {
                            int randNum = UnityEngine.Random.Range(0, spawnList.Count -1);
                        
                            float spawnX = spawnList[randNum].transform.position.x;
                            float spawnY = spawnList[randNum].transform.position.y;
                            float spawnZ = spawnList[randNum].transform.position.z;
                        
                            Vector3 playSpawn = new Vector3(spawnX, spawnY, spawnZ);
                        
                            obj.transform.position = playSpawn;
                        
                            obj.tag = "Dead";
                        };
                        
                        obj.GetComponent<HealthandShield>().CenterText.text = "DEFEAT";
                        obj.GetComponent<HealthandShield>().CenterText.color = Color.red;
                        numPlayerEliminated++;
                        
                    }

                }
            }

            for (int i = 0; i < PlayersInServer.Count; i++)
            {
                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(PlayersInServer[i], out NetworkObject netObj))
                {

                    GameObject obj = netObj.gameObject;

                    if (numPlayerEliminated > 0 && numPlayerEliminated == PlayersInServer.Count - 1 && obj.GetComponent<HealthandShield>().Health.Value > 0)
                    {
                        obj.GetComponent<HealthandShield>().CenterText.text = "VICTORY!";
                        obj.GetComponent<HealthandShield>().CenterText.color = Color.forestGreen;

                        StartCoroutine(switchScene());

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


    private IEnumerator switchScene()
    {

        yield return new WaitForSeconds(3);

        NetworkManager.SceneManager.LoadScene(menu, LoadSceneMode.Single);

    }


}
