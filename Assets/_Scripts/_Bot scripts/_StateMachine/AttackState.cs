using NUnit.Framework.Interfaces;
using UnityEngine;

public class AttackState : StateBase
{

    public float attackRate = 1.5f;   
    private float nextAttackTime = 0f;


    public AttackState(BotController ai) : base(ai) { }

    public override void Enter()
    {
         
         
        //ai.Agent.isStopped = true;
        ai.Animator.SetTrigger("Attack");
    }

    public override void Check()
    {
        if (ai.Player == null) return;
        ai.transform.LookAt(ai.Player.position);

        if (Time.time >= nextAttackTime)
        {
            ai.Attack();
            nextAttackTime = Time.time + attackRate;
        }
        if (ai.Agent.pathPending && ai.Agent.remainingDistance > ai.Agent.stoppingDistance)
            ai.ChangeState(new FollowState(ai));
        //if (ai.CanSeePlayer())
        //{
            

        //    //Vector3 dir = (ai.Player.position - ai.transform.position).normalized;
        //    //Quaternion lookRot = Quaternion.LookRotation(dir);
        //    //ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRot, Time.deltaTime * ai.rotationSpeed);
        //    ai.transform.LookAt(ai.Player.position);

        //    float dist = Vector3.Distance(ai.transform.position, ai.Player.position);
             
        //    if (dist > ai.attackRange)
        //    {
                
        //        ai.ChangeState(new FollowState(ai));
        //    }
        //}
        //else
        //{
        //    ai.ChangeState(new FollowState(ai));
        //}
    }

    public override void Leave()
    {
         
    }
}