using System;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
public class PlayerHealth : MonoBehaviourPun
{

    //public static PlayerHealth instance;

    public float health = 100;

    public event Action<float> OnHealthChange;
    public event Action PlayerHealthDestroyed;

    public GameObject playerUIPrefab;

     

    private AnimationHandler animationHandler;
    private PlayerInputHandler inputHandler;
    private LocalUIManager playerUI;
    private GunAttackHandler attackHandler;
    private GunPickupHandler pickupHandler;

    private GameManager gameManager;
    private bool isDead;
    private void Awake()
    {
        animationHandler=GetComponent<AnimationHandler>();
        inputHandler=GetComponent<PlayerInputHandler>();
        attackHandler=GetComponent<GunAttackHandler>();
        pickupHandler = GetComponent<GunPickupHandler>();
    }


    private void Start()
    {



        if (photonView.IsMine)
        {
            if (playerUIPrefab != null)
            {
                var UIGO = Instantiate(playerUIPrefab);
                playerUI = UIGO.GetComponent<LocalUIManager>();
                playerUI.SetTarget(this,photonView,attackHandler,pickupHandler);

            }
            else
            {
                 
            }

            gameManager=GameObject.FindAnyObjectByType(typeof(GameManager)).GetComponent<GameManager>();
        }

        
    }


    private void OnDestroy()
    {
        PlayerHealthDestroyed?.Invoke();
    }



    [PunRPC]
    public void Damage(float damage,int attackerId)
    {
         
    
        if(photonView.IsMine)
        {
            OnHealthChange?.Invoke(damage);
            health-=damage;
            Debug.Log(health);

            if (health <=0 && !isDead )
            {
                isDead = true;
                //animationHandler.SetDieState();
                photonView.RPC("Died", RpcTarget.All);

                // awarding score
                GlobalUIManager.instance.photonView.RPC("AddKill",RpcTarget.All,attackerId,photonView.Owner.ActorNumber);

                inputHandler.DisablingPlayerMap();
                playerUI.ShowLocalDeathUI();
                gameManager.ReSpawn(this.gameObject);
            }
        }
            
            
    }
}
