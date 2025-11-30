using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LeaveButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button leaveButton;

    private void Start()
    {
        leaveButton.onClick.AddListener(ReturnHome);
    }

    public void ReturnHome()
    {
         
        PhotonNetwork.Disconnect();       
    }


    public override void OnDisable()
    {
        base.OnDisable();
        leaveButton.onClick.RemoveListener(ReturnHome);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
         
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }

}

