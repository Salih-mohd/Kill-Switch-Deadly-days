using System;
using UnityEngine;
using UnityEngine.AI;


public class BotController : MonoBehaviour
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
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            var damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                //damageable.Damage(attackDamage);
                AttackEvent?.Invoke();
                Debug.Log("AI hit " + hit.collider.name + " for " + attackDamage + " damage");
            }
        }
    }

}
