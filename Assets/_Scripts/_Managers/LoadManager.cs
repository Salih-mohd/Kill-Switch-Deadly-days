using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviourPun
{
    public static LoadManager instance;
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;

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

    [PunRPC]
    public void ShowLoadUI(bool value)
    {

        loadingScreen.SetActive(value);
    }

    
    
}
