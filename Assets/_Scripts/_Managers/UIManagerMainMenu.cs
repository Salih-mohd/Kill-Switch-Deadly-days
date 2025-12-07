using System.Collections.Generic;
using System.Net.NetworkInformation;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMainMenu : MonoBehaviourPunCallbacks
{
    public static UIManagerMainMenu Instance;
    public bool inLobby;
    public bool connectedToMaster;

    public GameObject multiPlayerPanel;
    public GameObject connectErrorPanel;
    public GameObject playOptionPanel;
    public GameObject LobbyPanel;
    public GameObject playButtonLobbyPanel;
    public GameObject mainMenuUI;
    public GameObject createRoomOptions;
    public GameObject joinRoomOptions;



    [Header("error panels")]
    public GameObject roomCreatePanel;
    public GameObject joinRoomPanel;
    
    public TMP_InputField inputField;

    public List<TMP_Text> playerNameSlots;
    


    public GameManager gameManager;

    [Header("Room creation fields")]
    public TMP_InputField nameField;
    public TMP_InputField passWordField;
    public TMP_InputField maxPlayersField;

    [Header("Joining room fields")]
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private GameObject errorPanel;

    private string selectedRoomName;
    private string selectedRoomPassWord;



    private string roomName;
    private string passWord;
    private int maxPlayersCount;
    

    const string playerNamePrefKey = "PlayerName";

    string defaultName;


    private void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        nameField.onEndEdit.AddListener(SettingRoomName);
        passWordField.onEndEdit.AddListener(SettingPassword);
        maxPlayersField.onEndEdit.AddListener(SettingMaxPlayers);
        passwordInput.onEndEdit.AddListener(TryJoinRoom);
    }

    public override void OnDisable()
    {

        base.OnDisable();
        nameField.onEndEdit.RemoveListener(SettingRoomName);
        passWordField.onEndEdit.RemoveListener(SettingPassword);
        maxPlayersField.onEndEdit.RemoveListener(SettingMaxPlayers);
        passwordInput.onEndEdit.RemoveListener(TryJoinRoom);
    }

    private void Start()
    {
        defaultName= string.Empty;
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;

        gameManager =GameManager.instance;

    }

    public void ShowMuiltiplayerPanel()
    {
        multiPlayerPanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }

    public void ShowConnectErrorPanel()
    {
        connectErrorPanel.SetActive(true);
    }

    public void ShowPlayOptionPanel()
    {
        playOptionPanel.SetActive(true);
    }


    public void UpdatePlayerList()
    {
        // Clear all slots first
        foreach (var slot in playerNameSlots)
        {
            slot.text = "";
        }

        // Fill slots with current players
        int i = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (i < playerNameSlots.Count)
            {
                playerNameSlots[i].text = player.NickName.ToString(); // or player.ActorNumber
                i++;
            }
        }
    }

    public void SetPlayerName(string value)
    {
        
        if (string.IsNullOrEmpty(value))
        {
             
            return;
        }
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    public void ShowLobby(bool isMasterClient)
    {
        LobbyPanel.SetActive(true);
        playButtonLobbyPanel.SetActive(isMasterClient);
    }

    public void LoadArena()
    {
        gameManager.LoadArena();
    }


    // room creation

    private void SettingRoomName(string value)
    {
        roomName = value;
    }

    private void SettingPassword(string value)
    {
        passWord = value;
    }

    private void SettingMaxPlayers(string value)
    {
        maxPlayersCount =int.Parse(value);
    }

    public void CreateRoom()
    {
        if (!connectedToMaster)
        {
            roomCreatePanel.SetActive(true);
        }
        else
        {

            // Build room options
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)maxPlayersCount;
            options.CleanupCacheOnLeave = true;
            options.IsVisible = true;
            options.PlayerTtl = 50000;  
            options.EmptyRoomTtl = 60000;  


            // Store password in custom properties
            options.CustomRoomProperties = new Hashtable()
            {
                { "Password", passWord }
            };

             
            options.CustomRoomPropertiesForLobby = new string[] { "Password" };

            // Create the room
            PhotonNetwork.CreateRoom(roomName, options);
            //Debug.Log($"Creating room: {roomName} with max {maxPlayersCount} players and password {passWord}");
        }


    }





    // joining room set ups




    public void GoTOLobby()
    {

        if(connectedToMaster)
        {
            PhotonNetwork.JoinLobby();
            joinRoomOptions.SetActive(true);
        }
        else
        {
            joinRoomPanel.SetActive(true);
            joinRoomOptions.SetActive(false);
        }

        
    }
    public void QuiteLobby()
    {
        if (inLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        

    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform child in roomListParent) 
            Destroy(child.gameObject);

        foreach(RoomInfo room in roomList)
        {
            GameObject item = Instantiate(roomListItemPrefab, roomListParent);
            item.GetComponentInChildren<TMP_Text>().text = $"{room.Name}";

            Button joinButton = item.GetComponentInChildren<Button>();
            joinButton.onClick.AddListener(() =>
            {

                selectedRoomName=room.Name;
                selectedRoomPassWord = room.CustomProperties.ContainsKey("Password")
                ? room.CustomProperties["Password"].ToString() : "";

                passwordPanel.SetActive(true);
                
                
            });

        }
    }

    public void TryJoinRoom(string value)
    {

        if (connectedToMaster)
        {
            if (value == selectedRoomPassWord)
            {

                PhotonNetwork.JoinRoom(selectedRoomName);
                inLobby = false;
            }
            else
            {
                errorPanel.SetActive(true);
            }
        }
        else
        {
            roomCreatePanel.SetActive(true);
        }

        
    }


    public override void OnJoinedLobby()
    {
        inLobby = true;
    }

    public override void OnLeftLobby()
    {
        inLobby= false;
    }


    public void OnConnecctionLost()
    {
        multiPlayerPanel.SetActive(false);
        playOptionPanel.SetActive(false);
        mainMenuUI.SetActive(true );
        createRoomOptions.SetActive(false);
        joinRoomOptions.SetActive(false);   
    }


    public override void OnConnectedToMaster()
    {
        connectedToMaster = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectedToMaster=false;
    }

   

}
