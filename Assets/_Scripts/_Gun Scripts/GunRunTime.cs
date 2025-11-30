using Photon.Pun;
using UnityEngine;

public class GunRuntime : MonoBehaviourPun, IGun,IPunObservable
{
    [Header("Right Hand IK")]
    [SerializeField] public Transform ikRightHandIdlePos;
    [SerializeField] public Transform ikRightHandHintPos;

    [Header("Left Hand IK")]
    public Transform ikLeftHandIdlePos;
    public Transform ikLeftHandAimPos;

    [Header("Equip Settings")]
    [SerializeField] private string equipSocketName = "GunSocket";


    [Header("Gun stats")]
    public GunData gunData;
    //public BulletPool bulletPool;
    public Transform muzzleFlash;

    public int reserveAmmo=60;

    private AudioSource audioSource;
    private float lastFireTime;
    private int currentAmmo;


    // ******************
    //private GunTriggerDetector gunDetector;



    public float movementSpeedModifier=5f;
    public float leftIdleW = .2f;
    public float leftAimW = .6f;

    public Transform IKRightHandIdlePos => ikRightHandIdlePos;
    public Transform IKRightHandHintPos => ikRightHandHintPos;
    public Transform IKLeftHandIdlePos => ikLeftHandIdlePos;
    public Transform IKLeftHandAimPos => ikLeftHandAimPos;

    public float moveSpeed => movementSpeedModifier;

    public float LeftHIdleW => leftIdleW;

    public float LeftHAimW => leftAimW;

    public GunData GunData => gunData;
    public int CurrentAmmo
    {
        get => currentAmmo;
        set => currentAmmo = value;
    }

    public int RecerveAmmo
    {
        get=> reserveAmmo;
        set => reserveAmmo = value;
    }
    //public int RecerveAmmo { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //int IGun.CurrentAmmo { get; set; }

    private void Awake()
    {
        audioSource=GetComponent<AudioSource>();
    }

 
    public void OnEquip(Transform playerRoot)
    {
        EquipGun();
    }

    public void OnUnequip()
    {
        transform.SetParent(null);
         
    }




    // iPunObservable callbacks
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Owner sends ammo data
            stream.SendNext(currentAmmo);
            //stream.SendNext(gunData.reserveAmmo);
            stream.SendNext(reserveAmmo);
        }
        else
        {
            // Others receive ammo data
            currentAmmo = (int)stream.ReceiveNext();
            //gunData.reserveAmmo = (int)stream.ReceiveNext();
            reserveAmmo = (int)stream.ReceiveNext();
        }

    }

    private void EquipGun()
    {
        GameObject socket = GameObject.FindWithTag(equipSocketName);
        if (socket != null)
        {
            transform.SetParent(socket.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

        }
        else
        {
             
        }
    }

    [PunRPC]
    private void RPC_EquipGun(int playerViewID)
    {
        PhotonView playerPV = PhotonView.Find(playerViewID);
        if (playerPV == null)
        {
            
            return;
        }

        Transform socket = playerPV.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponPositions/"+equipSocketName);
        Debug.Log("socket name is "+socket.name);
        transform.SetParent(socket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        photonView.enabled = false;
        Debug.Log("equiped the gun on the player with id "+playerViewID);
    }


    [PunRPC]
    private void RPC_UnEquipGun()
    {
        transform.SetParent(null);
        photonView.enabled = true;
        
       
    }
}