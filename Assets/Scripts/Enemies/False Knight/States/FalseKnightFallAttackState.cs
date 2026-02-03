using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightFallAttackState : EnemyState
{
    FalseKnightController knight;
    public FalseKnightFallAttackState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }
    public override void EnterState()
    {
        knight.PlayAnimation("JumpAttack_FalseKnight");

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (knight.AnimationFinished && knight.IsGrounded())
        {
            knight.AnimationFinished = false;
            enemyStateMachine.ChangeState(knight.landState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
