using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalUIManager : MonoBehaviourPunCallbacks
{
    public Slider healthBar;
    public Image LocalScoreImage;
    public GlobalUIManager globalUI;
    public TMP_Text ammo;
    public Image gunImg;
     

    

    private PlayerHealth playerHealth;


    public event Action OnDiedEvent;


    private PhotonView playerView;

    private GunAttackHandler attackHandler;
    private GunPickupHandler pickupHandler;
    public override void OnDisable()
    {
        base.OnDisable();
        //playerHealth.OnHealthChange -= ChangeHealth;
        //PlayerDamage.instance.OnHealthChange -= ChangeHealth;

        if (playerHealth != null)
        {
            playerHealth.OnHealthChange -= ChangeHealth;
            playerHealth.PlayerHealthDestroyed -= DestroyPlayerUI;
        }

        if(attackHandler!=null)
        {
            attackHandler.OnAmmoChanged -= SettingUpAmmo;
        }
        if (pickupHandler != null)
        {
            pickupHandler.SentGunImg -= SettingUpGunImg;
            pickupHandler.RemoveGunImg -= RemoveGunImg;
        }


    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }


    public void ChangeHealth(float value)
    {
         
        healthBar.value-=value;

    }

    public void SetTarget(PlayerHealth target,PhotonView pv,GunAttackHandler attHandler,GunPickupHandler pickHand)
    {
        if (target == null)
        {
             
        }
            
        playerView=pv;
        playerHealth=target;
        playerHealth.OnHealthChange += ChangeHealth;
        playerHealth.PlayerHealthDestroyed += DestroyPlayerUI;
        attackHandler=attHandler;
        attackHandler.OnAmmoChanged += SettingUpAmmo;
        pickupHandler=pickHand;

        pickupHandler.SentGunImg += SettingUpGunImg;
        pickupHandler.RemoveGunImg += RemoveGunImg;
    }


    private void DestroyPlayerUI()
    {
        Destroy(this.gameObject);      
    }

    public void ShowLocalDeathUI()
    {
        LocalScoreImage.enabled = true;
    }


    private void SettingUpAmmo(int curr,int res)
    {
        ammo.text = $"{curr}/{res}";
    }

    private void SettingUpGunImg(Image img)
    {
        gunImg.sprite=img.sprite;
    }
    private void RemoveGunImg()
    {
        gunImg.sprite = null;
    }


    

    
}
