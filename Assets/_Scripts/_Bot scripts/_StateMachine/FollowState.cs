using UnityEngine;

public class FollowState : StateBase
{
    public FollowState(BotController ai) : base(ai) { }

    public override void Enter()
    {
        Debug.Log("AI entered Run state");
        ai.Agent.isStopped = false;
        ai.Animator.SetTrigger("Follow");
    }

    public override void Check()
    {
        ai.Agent.SetDestination(ai.Player.position);

        // Vision check
        if (ai.CanSeePlayer())
        {
            float dist = Vector3.Distance(ai.transform.position, ai.Player.position);
             
            if (dist <= ai.attackRange)
            {
                 
                ai.ChangeState(new AttackState(ai));
            }
        }
    }

    public override void Leave()
    {
        Debug.Log("AI leaving Run state");
    }
}