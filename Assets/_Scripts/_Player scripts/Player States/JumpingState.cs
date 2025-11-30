using UnityEngine;

public class JumpingState : BaseState
{
    public JumpingState(PlayerMovement playerMovement) : base(playerMovement) { }
    

    public override void Check()
    {
        Debug.Log("checking from jumping state");
    }

    public override void Enter()
    {
        Debug.Log("entered to jumping state");
    }

    public override void Exit()
    {
        Debug.Log("exited from jumping state");
    }
}
