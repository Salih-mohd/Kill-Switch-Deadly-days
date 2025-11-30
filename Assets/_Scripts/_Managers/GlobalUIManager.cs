using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviourPunCallbacks
{


    public static GlobalUIManager instance;

    public float matchDuration = 480f; // 8 minutes in seconds
    public TMP_Text timerText;

    public event Action OnMatchEnded;
    public GameObject ScoreBoard;

    //public GameObject scoreBoardd;

    public GameObject rowPrefab;
    public Transform rowParent;

    public bool gameOver;

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
    }


    public void ShowScoreBoard()
    {
        ScoreBoard.SetActive(true);
    }

    public void HideScoreBoard()
    {
        ScoreBoard.SetActive(false);
    }







    // score settings








    [PunRPC]
    public void AddKill(int killerId, int victimId)
    {
        // Get killer player object
        Player killer = PhotonNetwork.CurrentRoom.GetPlayer(killerId);
        if (killer == null) return;

        // Read current score
        int currentScore = killer.CustomProperties.ContainsKey("Score")
            ? (int)killer.CustomProperties["Score"]
            : 0;

        // Update score in CustomProperties (syncs across all clients)
        killer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", currentScore + 1 } });

         

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
    }






    // score board set up finished










}
