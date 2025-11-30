using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TargetHealth : MonoBehaviour, IDamage
{

    

    public float health=100;   
    public virtual void Damage(float damage,PhotonView pv,int killerID)
    {
        health-=damage;
        
    }
}
