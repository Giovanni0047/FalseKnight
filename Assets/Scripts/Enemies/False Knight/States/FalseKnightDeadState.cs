using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightDeadState : EnemyState
{
    FalseKnightController knight;
    public FalseKnightDeadState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("Dead_FalseKnight");
        knight.StopMovement();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
