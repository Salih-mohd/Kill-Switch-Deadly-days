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
        UIManagerMainMenu.Instance.connectedToMaster = true;
        UIManagerMainMenu.Instance.PlayPanelAndHideMainPanel();
        PhotonNetwork.JoinLobby();
    }

    



    public override void OnDisconnected(DisconnectCause cause)
    {
        UIManagerMainMenu.Instance.connectedToMaster = false;

         


        switch (cause)
        {
            case DisconnectCause.DnsExceptionOnConnect:
                ShowErrorUI();
                break;

            case DisconnectCause.ClientTimeout:
                //UIManagerMainMenu.Instance.ShowMessage("Connection timed out. Please check your network.");
                ShowErrorUI();
                break;

            case DisconnectCause.ServerTimeout:
                ShowErrorUI();
            //    UIManagerMainMenu.Instance.ShowMessage("Invalid AppId or authentication failed.");
                break;
            
            case  DisconnectCause.DisconnectByClientLogic:
                ShowErrorUI();
                break;

            case DisconnectCause.DisconnectByServerLogic:
                ShowErrorUI();
                break;

               

            default:
                //UIManagerMainMenu.Instance.ShowMessage("Disconnected: " + cause);
                ShowErrorUI();
                break;
        }
        //UIManagerMainMenu.Instance.ShowConnectErrorPanel();
        //UIManagerMainMenu.Instance.OnConnecctionLost();
    }


    private void ShowErrorUI()
    {
        UIManagerMainMenu.Instance.ShowConnectErrorPanel();
        UIManagerMainMenu.Instance.OnConnecctionLost();
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
        UIManagerMainMenu.Instance.roomCreatePanel.SetActive(true);
    }

    public override void OnConnected()
    {

        UIManagerMainMenu.Instance.ShowPlayOptionPanel();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        UIManagerMainMenu.Instance.joinRoomPanel.SetActive(true);
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

    //public void CreateRoom()
    //{
    //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });

    //}

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        UIManagerMainMenu.Instance.ShowMuiltiplayerPanel();
        PhotonNetwork.JoinLobby();
    }

    public void ShowMultiPlayerPanel()
    {
        if (PhotonNetwork.IsConnected)
        {
            UIManagerMainMenu.Instance.ShowMuiltiplayerPanel();
        }
        else
        {
            UIManagerMainMenu.Instance.ShowConnectErrorPanel();
        }
    }




     




}
