using UnityEngine;

public class LobbyButtons : MonoBehaviour
{

    public bool needPassword;
    public string lobbyID;



    public void JoinLobbyButtonPressed()
    {
        LobbyManager.Instance.JoinLobby(lobbyID, needPassword);
    }

}
