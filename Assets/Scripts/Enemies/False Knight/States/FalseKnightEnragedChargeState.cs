using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightEnragedChargeState : EnemyState
{
    FalseKnightController knight;
    int attackCount = 0;
    public FalseKnightEnragedChargeState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("EnragedCharge_FalseKnight");
        
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {
        if (knight.AnimationFinished)
        {
            knight.AnimationFinished = false;
            if (attackCount > 5)
            {
                attackCount = 0;
                knight.WasStunned = false;
                enemyStateMachine.ChangeState(knight.idleState);
            }
            else
            {
                knight.Flip();
                attackCount++;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
