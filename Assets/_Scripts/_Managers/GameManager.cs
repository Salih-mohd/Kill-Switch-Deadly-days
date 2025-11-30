using System;
using System.Collections;
using NUnit.Framework;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;
    public GameObject playerPrefab;
    public string LoadMangerName ;
    //public Slider healthBar;

    public PhotonView loadManagerPV;

    public Vector3[] spawnPostions;
    public Button LeaveButton;


    private bool inTheScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }




    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        //startedToLoad += ShowLoadingUI;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (LeaveButton!=null)
        {
            LeaveButton.onClick.RemoveAllListeners();
        }
        //startedToLoad-= ShowLoadingUI;
    }


    public void LoadArena()
    {

        loadManagerPV.RPC("ShowLoadUI",RpcTarget.All,true);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }


    }


    

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {



            int index = PhotonNetwork.LocalPlayer.ActorNumber % spawnPostions.Length;
            Vector3 spawnPos = spawnPostions[index];



            PhotonNetwork.Instantiate("playerPrefabAllSet", spawnPos, Quaternion.identity);
            //timerText = GameObject.FindWithTag("TimerText").GetComponent<TMP_Text>();
            if (PhotonNetwork.IsMasterClient)
            {
                GlobalUIManager.instance.StartingTime();
            }
            inTheScene = true;


            loadManagerPV.RPC("ShowLoadUI", RpcTarget.All, false);
            StartCoroutine(DestroyLoadManger());

        }
    }

    public void ReSpawn(GameObject prefab)
    {
        Debug.Log("inside reSpawn");
        StartCoroutine(ReSpawn_Coroutine(prefab));
    }

    IEnumerator ReSpawn_Coroutine(GameObject prefab)
    {
        int index = PhotonNetwork.LocalPlayer.ActorNumber % spawnPostions.Length;
        Vector3 spawnPos = spawnPostions[index];
        yield return new WaitForSeconds(7);
         
        PhotonNetwork.Destroy(prefab);
        PhotonNetwork.Instantiate("playerPrefabAllSet", spawnPos, Quaternion.identity);       
    }


    public void ReturnHome()
    {
        SceneManager.LoadSceneAsync(0);
         
    }

    public void GettingLoadManager()
    {
        var obj = PhotonNetwork.Instantiate(LoadMangerName, new Vector3(0, 0, 0), Quaternion.identity);
        loadManagerPV=obj.GetComponent<PhotonView>();

    }


    IEnumerator DestroyLoadManger()
    {
        yield return new WaitForSeconds(15);
        if(loadManagerPV != null ) 
        Destroy(loadManagerPV.gameObject);
    }



}
