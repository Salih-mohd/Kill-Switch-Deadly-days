using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMainMenu : MonoBehaviour
{
    public static UIManagerMainMenu Instance;
    public GameObject multiPlayerPanel;
    public GameObject connectErrorPanel;
    public GameObject playOptionPanel;
    public GameObject LobbyPanel;
    public GameObject playButtonLobbyPanel;

    public TMP_InputField inputField;

    public List<TMP_Text> playerNameSlots;
    

    const string playerNamePrefKey = "PlayerName";

    string defaultName;


    private void Awake()
    {
        Instance = this;
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

}
