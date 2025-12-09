using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UIElements;

public class TargetFX : MonoBehaviour, IFX,IOnEventCallback
{
    public FXPool fxPool;

    public int fxEvent;

    public int FXEvent => fxEvent;


    public string tagOfPool;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    private void Start()
    {
        if(tagOfPool!=string.Empty)
            fxPool = GameObject.FindWithTag(tagOfPool).GetComponent<FXPool>();

    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == FXEvent) // Our FX event
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 pos = (Vector3)data[0];
            Quaternion rot = (Quaternion)data[1];

            // Play FX locally on receiving clients
            PlayFx(pos, rot);
        }

    }

    public void PlayFx(Vector3 position,Quaternion rotation)
    {
        if (fxPool != null)
            fxPool.PlayFX(position, rotation);
        
    }
}
