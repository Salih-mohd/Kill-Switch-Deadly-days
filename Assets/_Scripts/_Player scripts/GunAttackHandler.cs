using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class GunAttackHandler : MonoBehaviourPun
{

    // public fields
    public bool isAttacking;

    // assighned via player movement
    [HideInInspector] public float attackingSpeed;
    [HideInInspector] public float gunHoldingSpeed;
    // events
    public event Action<float> AttackEvent;
    public event Action<float> AttackDoneEvent;


    public Transform debugTransform;
    public GameObject bulletPrefab;
    public GameObject spawnPosition;
    public LayerMask bulletMask;
    public GameObject fxPrefab;




    // private fields
    private GunPickupHandler gunPickupHandler;
    private GunRigController gunRigController;
    private AnimationHandler animationHandler;
    private PlayerInputHandler inputHandler;


    private bool wasAttacking;
    private bool isReloading;
    private bool noAmmo;
    private bool hasPlayedEmptySound;

    private IGun gun;
    private GunData gunData;


    private float lastFireTime = 0;
 
    private RaycastHit hit;


    private AudioSource audioSource;
    [SerializeField] private AudioSource reloadAudioSource;
    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private AudioClip reloadSound;

    public event Action<int, int> OnAmmoChanged;

    private void Awake()
    {
        // testing to delete it.
        //PhotonNetwork.OfflineMode = true;

        gunPickupHandler = GetComponent<GunPickupHandler>();
        gunRigController = GetComponent<GunRigController>();
        animationHandler = GetComponent<AnimationHandler>();
        inputHandler = GetComponent<PlayerInputHandler>();
        audioSource = GetComponent<AudioSource>();
        

    }

    private void OnEnable()
    {
        inputHandler.ReloadEvent += Reload;
    }

    private void OnDisable()
    {
        inputHandler.ReloadEvent -= Reload;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

       
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
       


        if (gunPickupHandler.HasGunEquipped)
        {
            gun = gunPickupHandler.currentGun;
            gunData = gun.GunData;
            

            // audio playing.
            audioSource.resource=gunData.fireSound;


            if (inputHandler.fire && !isReloading)
            {              
                if(Time.time-lastFireTime>= gunData.fireRate)
                {
                    lastFireTime = Time.time;
                    Fire(ray);
                }
            }


            if (inputHandler.isAttacking && !isReloading)
            {




                // Local call
                AttackEventHandler();
                gunRigController.UpdateLeftHandAim(true);
                isAttacking = true;
                //RPC_HandlingAttackingRigAndBool();
                
                // remote call
                if(PhotonNetwork.IsConnected)
                {
                    photonView.RPC("RPC_HandlingAttackingRigAndBool", RpcTarget.All);
                    photonView.RPC("AttackEventHandler", RpcTarget.Others);
                }
                


                animationHandler.TriggerAttack();
                animationHandler.SetAttackState(true);
                
            }
            else if (inputHandler.isNotAttacking)
            {



                //Local call
                AttackEventHandlerr();
                //RPC_HandlingNotAttackingRigAndBool();
                gunRigController.UpdateLeftHandAim(false);
                isAttacking = false;

                // Remote call
                photonView.RPC("RPC_HandlingNotAttackingRigAndBool", RpcTarget.All);
                photonView.RPC("AttackEventHandlerr", RpcTarget.Others);


                animationHandler.SetAttackState(false);
                

            }

        }


        if (!isAttacking)
        {
            reloadAudioSource.resource = null;
        }



    }


    [PunRPC]
    private void AttackEventHandler()
    {
        if (wasAttacking) return;
        AttackEvent?.Invoke(attackingSpeed);
        wasAttacking=!wasAttacking;
        
    }


    [PunRPC]
    private void AttackEventHandlerr()
    {
        if(!wasAttacking) return;
        AttackDoneEvent?.Invoke(gunHoldingSpeed);
        wasAttacking = !wasAttacking;
        

    }

    private void Fire(Ray ray)
    {

        if (gun == null || gunData == null)
        {
             
            return;
        }



        if (Physics.Raycast(ray, out hit, 999f, bulletMask))
        {
            debugTransform.position = hit.point;
        }


        if (gun.CurrentAmmo > 0)
        {
            gun.CurrentAmmo--;
            //Debug.Log(gun.CurrentAmmo);
            OnAmmoChanged?.Invoke(gun.CurrentAmmo, gun.RecerveAmmo);


            Vector3 fxSpawnPos = hit.point + hit.normal * 0.05f;
            Quaternion fxRotation = Quaternion.LookRotation(hit.normal);



            var targetHealth = hit.collider.GetComponent<IDamage>();
            var targetPVHealth = hit.collider.GetComponentInParent<PhotonView>();

            //Local call
            if(targetPVHealth != null) targetHealth?.Damage(gunData.damage, targetPVHealth,photonView.Owner.ActorNumber);

            //network call
            //if (targetPVHealth!=null)
            //{
            //    targetPVHealth.RPC("Damage",targetPVHealth.Owner,gunData.damage);
            //}



            var targetFx = hit.collider.GetComponent<IFX>();
            //Local
            targetFx?.PlayFx(fxSpawnPos, fxRotation);
            // broadcast 
            if (PhotonNetwork.InRoom && targetFx!=null)
            {
                object[] content = new object[] { fxSpawnPos, fxRotation };
                RaiseEventOptions options = new() { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((byte)targetFx.FXEvent, content, options, SendOptions.SendReliable);
            }






            //photonView.RPC("RPC_HitFX", RpcTarget.All, targetFx, fxSpawnPos, fxRotation);

        }
        else
        {

            // local clal
            audioSource.Stop();
            // network call
            photonView.RPC("RPC_StopAudio", RpcTarget.Others);

            if (gunData.reserveAmmo>0)
            {
                noAmmo = false;
                
                // local call
                Reload();
                //// network clal
                //photonView.RPC("RPC_Reload", RpcTarget.Others);
            }
            else
            {
                noAmmo = true;
                if (!hasPlayedEmptySound)
                {
                    // Local call
                    reloadAudioSource.resource = noAmmoSound;
                    reloadAudioSource.Play();
                    // network call
                    //photonView.RPC("RPC_PlayEmptySound",RpcTarget.Others);


                    hasPlayedEmptySound = true;
                }

            }

        }
    }

    

    public void Reload()
    {
        isReloading = true;


        //Local
        audioSource.Stop();
        // global
        photonView.RPC("RPC_StopAudio",RpcTarget.Others);


        StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {

        reloadAudioSource.resource = null;
        // local call
        reloadAudioSource.PlayOneShot(gunData.reloadSound);
        // global call
        photonView.RPC("RPC_ReloadHandling", RpcTarget.Others);
        yield return new WaitForSeconds(2);
        hasPlayedEmptySound = false;
        

        // setting up ammos 

        int needed = gunData.maxAmmo - gun.CurrentAmmo;
        //int reloadAmount = Mathf.Min(needed, gunData.reserveAmmo);
        int reloadAmount = Mathf.Min(needed, gun.RecerveAmmo);

        gun.CurrentAmmo += reloadAmount;
        gun.RecerveAmmo -= reloadAmount;

        OnAmmoChanged?.Invoke(gun.CurrentAmmo, gun.RecerveAmmo);

        isReloading = false;
        
        if (isAttacking)
        {
            // local call
            audioSource.Play();
            // global call
            photonView.RPC("RPC_PlayAudio", RpcTarget.Others);
        }


    }


    [PunRPC]
    private void RPC_HandlingAttackingRigAndBool()
    {
        
        //gunRigController.UpdateLeftHandAim(true);
        //isAttacking = true;

        if (!noAmmo)
        {
            audioSource.Play();
        }
    }


    [PunRPC]
    private void RPC_HandlingNotAttackingRigAndBool()
    {
        audioSource.Stop();
        //gunRigController.UpdateLeftHandAim(false);
        //isAttacking = false;

        hasPlayedEmptySound = false;
    }

    [PunRPC] 
    private void RPC_StopAudio()
    {
        audioSource.Stop();
    }

    [PunRPC]
    private void RPC_PlayAudio()
    {
        audioSource.Play();
    }


    [PunRPC]
    private void RPC_PlayEmptySound()
    {
        reloadAudioSource.resource = noAmmoSound;
        reloadAudioSource.Play();
    }

    [PunRPC]
    private void RPC_ReloadHandling()
    {
        reloadAudioSource.PlayOneShot(reloadSound);
    }



    public void InvokingAmmoChangingEvent(int curr,int reserve)
    {
        OnAmmoChanged?.Invoke(curr, reserve);
    }

}
