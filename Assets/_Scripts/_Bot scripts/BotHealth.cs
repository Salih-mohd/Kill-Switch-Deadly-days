using Photon.Pun;
using UnityEngine;

public class BotHealth : MonoBehaviourPun,IPunObservable
{
    public int health = 0;
    public BotController controller;
    PhotonView botPv;
    private bool isDead;


    private void Awake()
    {
        controller = GetComponent<BotController>();
        botPv=GetComponent<PhotonView>();
    }
    public void DecreaseHealth(int value)
    {
        if (isDead) return;
        health -= value;
        //Debug.Log(health);
        if(health <= 0)
        {
            isDead = true;
            //Debug.Log("bot died");
            //controller.ChangeState(new DieState(controller));
            botPv.RPC("RPC_Die", RpcTarget.All);
            //controller.ChangeState(new DieState(controller));

        }
    }

     



    // Sync health across network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(isDead);
        }
        else
        {
            health = (int)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
        }
    }


    [PunRPC]
    private void RPC_Die()
    {
        //Debug.Log("Bot died");
        controller.ChangeState(new DieState(controller));
    }


}
