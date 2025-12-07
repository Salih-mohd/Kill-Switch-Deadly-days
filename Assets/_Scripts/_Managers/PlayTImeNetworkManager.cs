using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayTImeNetworkManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        otherPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", null } });

        //otherPlayer.SetCustomProperties(new Hashtable());
        //PhotonNetwork.DestroyPlayerObjects(otherPlayer);


        GlobalUIManager.instance.AddLeaveEntry(otherPlayer.NickName);


    }
}
