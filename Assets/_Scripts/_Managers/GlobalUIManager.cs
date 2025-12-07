using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviourPunCallbacks
{


    public static GlobalUIManager instance;

    public float matchDuration = 480f; 
    public TMP_Text timerText;

    public event Action OnMatchEnded;
    public GameObject ScoreBoard;
    public GameObject scoreBoardButtons;

    //public GameObject scoreBoardd;

    public GameObject rowPrefab;
    public Transform rowParent;

    public bool gameOver;
    public GameObject errorPanel;
    public GameObject failPanel;

    [Header("feed datas")]
    public Transform feedParent;
    public Transform hitFeedParent;

    public GameObject feedEntryPrefab;
    private void Awake()
    {
        instance= this;
    }



    private void Start()
    {
        BuildInitialBoard();

    }


    public override void OnEnable()
    {
        base.OnEnable();
        OnMatchEnded += ShowScoreBoard;

    }
    public override void OnDisable()
    {
        base.OnDisable();
        OnMatchEnded -= ShowScoreBoard;
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null &&
        PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime"))
        {
            double startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
            double elapsed = PhotonNetwork.Time - startTime;
            double remaining = matchDuration - elapsed;

            if (remaining < 0) remaining = 0;

            int minutes = (int)(remaining / 60);
            int seconds = (int)(remaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";

            if (remaining <= 0)
            {
                if (!gameOver)
                {
                    OnMatchEnded?.Invoke();
                    gameOver = true;
                }

            }

        }
    }


    // timer settings

    public void StartingTime()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["StartTime"] = PhotonNetwork.Time;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);


        // clearing the score property when game start.
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                p.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", 0 } });
            }
        }

    }


    public void ShowScoreBoard()
    {
        ScoreBoard.SetActive(true);
        scoreBoardButtons.SetActive(true);
    }

    public void HideScoreBoard()
    {
        ScoreBoard.SetActive(false);
        scoreBoardButtons.SetActive(false);
    }







    // score settings








    [PunRPC]
    public void AddKill(int killerId, int victimId)
    {
        // Get killer player object
        Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        Player victim=PhotonNetwork.CurrentRoom.GetPlayer(victimId);
        if (killer == null) return;

        // Read current score
        int currentScore = killer.CustomProperties.ContainsKey("Score")
            ? (int)killer.CustomProperties["Score"]
            : 0;

        // Update score in CustomProperties (syncs across all clients)
        killer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", currentScore + 1 } });

        AddFeedEntry(killer.NickName, victim.NickName);

        // Raise local event for UI systems
        //OnKillAdded?.Invoke(killerId, victimId, currentScore + 1);
    }









    //Score board set up





    private void BuildInitialBoard()
    {
        foreach (Transform child in rowParent)
        {
            if (child.CompareTag("ProtectedButton"))
                continue;

            Destroy(child.gameObject);
        }
            

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject row = Instantiate(rowPrefab, rowParent);
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            string name = string.IsNullOrEmpty(player.NickName) ? $"Player {player.ActorNumber}" : player.NickName;
            texts[0].text = name;
            texts[1].text = "0"; // start at zero
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Score"))
        {
            RefreshScore(targetPlayer);
        }
    }


    void RefreshScore(Player player)
    {
        foreach (TMP_Text text in rowParent.GetComponentsInChildren<TMP_Text>())
        {
            if (text.text == player.NickName || text.text == $"Player {player.ActorNumber}")
            {
                // Find sibling TMP_Text (score field)
                TMP_Text scoreText = text.transform.parent.GetComponentsInChildren<TMP_Text>()[1];
                int score = player.CustomProperties.ContainsKey("Score") ? (int)player.CustomProperties["Score"] : 0;
                scoreText.text = score.ToString();
            }
        }

        SortScoreBoard();
    }






    // score board set up finished



    public override void OnDisconnected(DisconnectCause cause)
    {
        errorPanel.SetActive(true);

    }

    public void Reconnect()
    {
        Debug.Log("reconnect is called");
        if (!PhotonNetwork.ReconnectAndRejoin())
        {
            Debug.Log("shwoing error panel");
            failPanel.SetActive(true);
            failPanel.SetActive(true);
        }
        else
        {
            Debug.Log("disabling error panels because reconnected");
            failPanel.SetActive(false);
            errorPanel.SetActive(false);
        }
            
    }

    public void ReturnToHome()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync(0);
    }


    // kill feed management
    public void AddFeedEntry(string killerName, string victimName)
    {
        GameObject entry = Instantiate(feedEntryPrefab, feedParent);
        TMP_Text text = entry.GetComponentInChildren<TMP_Text>();
        text.text = $"{killerName} killed {victimName}";
    }

    //leaving room notification
    public void AddLeaveEntry(string playerName)
    {
        GameObject entry = Instantiate(feedEntryPrefab, feedParent);
        var text= entry.GetComponentInChildren<TMP_Text>();
        text.text=$"{playerName} left room";
    }


    // hit marker notification
    public void HitMarkUpdate(float damage)
    {

        Debug.Log("called hit marck updatte method");
        GameObject entry=Instantiate(feedEntryPrefab, feedParent);
        var txt= entry.GetComponentInChildren<TMP_Text>();
        txt.text = $"Took {damage} damage";
    }

    private void SortScoreBoard()
    {
        var rows = new List<(Transform row, int score)>();
        foreach (Transform row in rowParent)
        {
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 2)
            {
                int score;
                if (int.TryParse(texts[1].text, out score))
                {
                    rows.Add((row, score));
                }
            }
        }

        rows = rows.OrderByDescending(r => r.score).ToList();

        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].row.SetSiblingIndex(i);
        }
    }



}
