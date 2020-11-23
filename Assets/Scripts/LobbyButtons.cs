using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class LobbyButtons : MonoBehaviour
{
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Blocking");
    }

    public void QuitGame()
    {
        NetworkManager.singleton.ServerChangeScene("Blocking");
        LeaveRoom();
    }
}
