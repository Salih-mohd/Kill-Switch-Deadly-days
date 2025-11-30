using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GunPickupHandler : MonoBehaviourPun
{

    // private variables
    private GameObject nearbyGun;
    

    [Header("Dependencies")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private GunRigController rigController;
    [SerializeField] private AnimationHandler animationHandler;
    private GunAttackHandler attackHandler;
    private Inventory inventory;
    private GunData gundata;
    



    //public variables
    public bool HasGunEquipped;
    public IGun currentGun;
    

    //events
    public event Action<float> gunPicked;
    public event Action<float> gunDropped;
    public event Action<Image> SentGunImg;
    public event Action RemoveGunImg;


    public float speedWithoughtGun;



    private PhotonView pv;

    private GameObject gunGameobject;
    private PhotonView gunPV;



    private void Awake()
    {
        pv=GetComponent<PhotonView>();
        inventory=GetComponent<Inventory>();
        attackHandler = GetComponent<GunAttackHandler>();
    }

    private void OnEnable()
    {
        inventory.AmmoAdded += HandleAmmoAdded;
    }

    private void OnDisable()
    {
        inventory.AmmoAdded -= HandleAmmoAdded;
    }

    public void SetNearbyGun(GameObject gun)
    {
        nearbyGun = gun;
    }

    public void ClearNearbyGun()
    {
        nearbyGun = null;
    }

    private void Update()
    {

        if (!pv.IsMine) return;

        HandlePickup();
        HandleDrop();


        
    }

    private void HandlePickup()
    {
        if (inputHandler.GrabbedThisFrame && nearbyGun != null)
        {
            currentGun = nearbyGun.GetComponent<IGun>();
            gundata=currentGun.GunData;
            gunGameobject = nearbyGun;
            SentGunImg?.Invoke(gundata.gunImg);
            
            if (currentGun != null)            
            {
                // transferring owner ship of gun
                gunPV=gunGameobject.GetComponent<PhotonView>();
                if (gunPV != null)
                {
                    gunPV.RequestOwnership();
                     
                }


                currentGun.CurrentAmmo = currentGun.GunData.maxAmmo;
                currentGun.RecerveAmmo += inventory.GetReserveAmmo(gundata.ammoName);


                attackHandler.InvokingAmmoChangingEvent(currentGun.CurrentAmmo, currentGun.RecerveAmmo);




                 

                gunPicked.Invoke(currentGun.moveSpeed);          
                HasGunEquipped = true;

                //currentGun.OnEquip(transform);
                gunPV.RPC("RPC_EquipGun", RpcTarget.All, photonView.ViewID);
                
                rigController.ApplyIK(currentGun);

                animationHandler.SetGunState(true);
                
            }
        }
    }

    private void HandleDrop()
    {
        if (inputHandler.DetachedThisFrame && currentGun != null)
        {
            
            attackHandler.InvokingAmmoChangingEvent(0, 0);

            RemoveGunImg?.Invoke();

             
            gunDropped.Invoke(speedWithoughtGun);

            
            //currentGun.OnUnequip();
            gunPV.RPC("RPC_UnEquipGun", RpcTarget.All);
            HasGunEquipped=false;
           

            rigController.ClearIK();

            animationHandler.SetGunState(false);

            currentGun = null;
            
            gunGameobject = null;

            gunPV.TransferOwnership(PhotonNetwork.MasterClient);

            gunPV = null;
            gundata = null;


        }
    }

    private void HandleAmmoAdded(string ammoName,int count)
    {
        if(currentGun != null)
        {
            currentGun.RecerveAmmo += inventory.GetReserveAmmo(gundata.ammoName);
            attackHandler.InvokingAmmoChangingEvent(currentGun.CurrentAmmo, currentGun.RecerveAmmo);
        }
    }
}