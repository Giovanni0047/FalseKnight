using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightStunEndState : EnemyState
{
    FalseKnightController knight;
    private float startTime;
    private float cooldownChangerState = 3;
    public FalseKnightStunEndState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }
    public override void EnterState()
    {
        knight.PlayAnimation("Stun(RollEnd)_FalseKnight");
        startTime = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (Time.time - startTime > cooldownChangerState)
        {
            enemyStateMachine.ChangeState(knight.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
