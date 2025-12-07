using System;
using System.Collections;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviourPunCallbacks
{
    public NavMeshAgent Agent;
    public Animator Animator;
    public Transform Player;
    public Transform rayOrigin;
    public Transform attackOrigin;


    public float attackRange = 2f;
    public float visionRange = 100f;
    public float rotationSpeed = 5f;
    public float attackDamage = 10f;

    public event Action AttackEvent;
    

    private StateBase currentState;
    
    



    private void Awake()
    {
        
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {      
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Check();
    }

    public void ChangeState(StateBase newState)
    {
        currentState?.Leave();
        currentState = newState;
        currentState.Enter();
    }

    public bool CanSeePlayer()
    {
        //Vector3 dir = (Player.position - transform.position).normalized;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, visionRange))
        {
            
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    public void Attack()
    {
        Ray ray = new(attackOrigin.position, attackOrigin.forward);
        AttackEvent?.Invoke();
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            var damageable = hit.collider.GetComponent<IDamage>();
            var targetPVHealth = hit.collider.GetComponentInParent<PhotonView>();
            if (damageable != null)
            {
                if (targetPVHealth != null)
                {
                    damageable.DamageForBot(.5f, targetPVHealth,this.photonView);
                }

                AttackEvent?.Invoke();
                //Debug.Log("AI hit " + hit.collider.name + " for " + attackDamage + " damage");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.transform;


            //Agent.SetDestination(Player.position);
            ChangeState(new FollowState(this));
             
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Player == other.transform)
        {
            Player = null;
            ChangeState(new IdleState(this));
            
        }
    }

    public void DestroyAfterDeath()
    {
        StartCoroutine(Coro_Died());
        //Debug.Log("called destroy after death");
    }

    IEnumerator Coro_Died()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }



    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
         
        if (Player != null)
        {
            PhotonView pv = Player.GetComponent<PhotonView>();
            if (pv != null && pv.Owner == otherPlayer)
            {
                Player = null;
                ChangeState(new IdleState(this));
            }
        }
    }

    [PunRPC]
    public void ChangeToIdle()
    {
        ChangeState(new IdleState(this));
    }



}
