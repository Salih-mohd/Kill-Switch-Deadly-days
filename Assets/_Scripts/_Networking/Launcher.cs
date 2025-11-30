using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    //public variables
    public int maxPlayers = 8;

    //private variables
    private string gameVersion = "1";

    


    // MonoBehaviourPunCallback method session

    public override void OnConnectedToMaster()
    {
         
        //GameManager.instance.GettingLoadManager();
    }

    

    public override void OnDisconnected(DisconnectCause cause)
    {
         
        UIManagerMainMenu.Instance.ShowConnectErrorPanel();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
         
    }
    
    public override void OnJoinedRoom()
    {
         
        UIManagerMainMenu.Instance.ShowLobby(PhotonNetwork.IsMasterClient);
        UIManagerMainMenu.Instance.UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UIManagerMainMenu.Instance.UpdatePlayerList();

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UIManagerMainMenu.Instance.UpdatePlayerList();

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
         
    }

    public override void OnConnected()
    {
         
        UIManagerMainMenu.Instance.ShowPlayOptionPanel();
    }

    


    // ending MonoBehaviourPunCallback session

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
        
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });
        
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        UIManagerMainMenu.Instance.ShowMuiltiplayerPanel();
    }

    public void ShowMultiPlayerPanel()
    {
        if(PhotonNetwork.IsConnected)
        {
            UIManagerMainMenu.Instance.ShowMuiltiplayerPanel();
        }
        else
        {
            UIManagerMainMenu.Instance.ShowConnectErrorPanel();
        }
    }

}
