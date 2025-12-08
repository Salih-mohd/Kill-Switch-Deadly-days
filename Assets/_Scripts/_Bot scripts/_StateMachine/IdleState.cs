using NUnit.Framework.Interfaces;
using UnityEngine;

public class IdleState : StateBase
{
    public IdleState(BotController ai) : base(ai) { }

    public override void Enter()
    {

         
        ai.Animator.SetTrigger("Idle");
        //ai.Animator.SetBool("IdleBool",true );
        ai.Agent.isStopped = true;  
    }

    public override void Check()
    {
        
        //ai.ChangeState(new FollowState(ai));
    }

    public override void Leave()
    {
        ai.Agent.isStopped = false;
    }
}