using UnityEngine;
public class FalseKnightIdleState : EnemyState
{
    FalseKnightController knight;
    private float startTime = 0f;
    private float cooldownChangeState = 2f;
    public FalseKnightIdleState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("Idle_FalseKnight");
        startTime = Time.time;
        knight.StopMovement();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (Time.time - startTime > cooldownChangeState && GameManager.Instance.IsPlayerDead == false)
        {
            if (knight.WasStunned)
            {
                enemyStateMachine.ChangeState(knight.jumpAnticipate);
            }
            else if (knight.DistanceToPlayer() > knight.longJumpDistance)
            {
                if (Random.value > 0.5)
                {
                    enemyStateMachine.ChangeState(knight.attackState);
                }
                else
                {
                    enemyStateMachine.ChangeState(knight.runState);
                }
            }
            else
            {
                if (Random.value > 0.6) 
                {
                    enemyStateMachine.ChangeState(knight.attackState);
                }
                else
                {
                    enemyStateMachine.ChangeState(knight.jumpAnticipate);
                }
            }
        }
        knight.FlipToPlayer();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
