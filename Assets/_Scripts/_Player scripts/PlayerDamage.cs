using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerDamage : MonoBehaviourPun, IDamage
{
    public float dam;
    //public event Action<float> OnHealthChange;
    //public static PlayerDamage instance;

    // test

    public TargetFX bloodFX;


    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = transform.parent.root.GetComponent<PlayerHealth>();
        //instance = this;
    }

    private void Start()
    {
        bloodFX = GetComponent<TargetFX>();
        bloodFX.fxPool = GameObject.FindWithTag("BloodPool").GetComponent<FXPool>();
    }


    public void Damage(float damage, PhotonView pv, int attackerId)
    {

        //var obj=Instantiate(damageText);
        //obj.transform.SetParent(GameObject.FindWithTag("PlayerUI").transform);

        //obj.GetComponent<TMP_Text>().text = $"Player took {damage}";

        //StartCoroutine(DamTxtCoolDown(obj));

        //damageText.enabled = true;
        //damageText.text = $"Player took {damage}";



        //if (!photonView.IsMine) return;

        damage += dam;
        //OnHealthChange?.Invoke(damage);
        pv.RPC("Damage", pv.Owner, damage, attackerId);


        // hit marker update
        
        
        //GlobalUIManager.instance.HitMarkUpdate(dam);
        
        //playerHealth.Damage(damage);        
    }

    public void DamageForBot(float damage, PhotonView pv,PhotonView botPV)
    {
        damage += dam;
        pv.RPC("Damagee", pv.Owner, damage,botPV.ViewID);
    }

    

    // test

    //IEnumerator DamTxtCoolDown(GameObject prfb)
    //{
    //    yield return new WaitForSeconds(1);
    //    //damageText.enabled = false;
    //}
}
