using System;
using Photon.Pun;
using UnityEngine;

public class PlayerDamage : MonoBehaviourPun,IDamage
{  
    public float dam;
    //public event Action<float> OnHealthChange;
    //public static PlayerDamage instance;


    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = transform.parent.root.GetComponent<PlayerHealth>();
        //instance = this;
    }

    
    public void Damage(float damage,PhotonView pv,int attackerId)
    {
        
        //if (!photonView.IsMine) return;
         
        damage += dam; 
        //OnHealthChange?.Invoke(damage);
        pv.RPC("Damage",pv.Owner,damage,attackerId);
        //playerHealth.Damage(damage);        
    }
}
