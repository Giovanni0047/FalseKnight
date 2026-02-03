using UnityEngine;

public class FalseKnightStunState : EnemyState
{
    FalseKnightController knight;
    public FalseKnightStunState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("Stun_FalseKnight");
        knight.StopMovement();
        knight.Jump(knight.jumpStun,false);
        knight.WasStunned = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (knight.IsGrounded() && knight.AnimationFinished)
        {
            knight.AnimationFinished = false;
            enemyStateMachine.ChangeState(knight.stunEndState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
