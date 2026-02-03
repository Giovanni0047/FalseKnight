using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightAttackState : EnemyState
{
    FalseKnightController knight;

    public FalseKnightAttackState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }
    public override void EnterState()
    {
        knight.PlayAnimation("Attack_FalseKnight");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (knight.AnimationFinished)
        {
            knight.AnimationFinished = false;
            enemyStateMachine.ChangeState(knight.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
