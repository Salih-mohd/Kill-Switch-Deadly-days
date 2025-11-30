using NUnit.Framework.Interfaces;
using UnityEngine;

public class AttackState : StateBase
{
    public AttackState(BotController ai) : base(ai) { }

    public override void Enter()
    {
         
        ai.Agent.isStopped = true;
        ai.Animator.SetTrigger("Attack");
    }

    public override void Check()
    {
        if (ai.CanSeePlayer())
        {
             
            Vector3 dir = (ai.Player.position - ai.transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, lookRot, Time.deltaTime * ai.rotationSpeed);

            float dist = Vector3.Distance(ai.transform.position, ai.Player.position);
             
            if (dist > ai.attackRange)
            {
                
                ai.ChangeState(new FollowState(ai));
            }
        }
        else
        {
            ai.ChangeState(new FollowState(ai));
        }
    }

    public override void Leave()
    {
         
    }
}