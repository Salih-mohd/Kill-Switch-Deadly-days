using Photon.Pun;
using UnityEngine;

public interface IDamage
{
    void Damage(float damage,PhotonView pv,int attackerId);

    
}
