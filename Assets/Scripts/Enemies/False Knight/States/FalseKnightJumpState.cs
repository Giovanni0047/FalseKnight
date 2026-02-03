using UnityEngine;

public class FalseKnightJumpState : EnemyState
{
    FalseKnightController knight;
    private float startTime;
    private float cooldownChangerState = 0.2f;

    public FalseKnightJumpState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }
    public override void EnterState()
    {
        startTime = Time.time;

        if (knight.WasStunned)
        {
            knight.Jump(new Vector2(knight.DistanceToCenterPoint(), 18), knight.CenterPointIsLeft());
        }
        else
        {
            if (knight.DistanceToPlayer() < knight.shortJumpDistance && knight.Running == false)
            {
                knight.ShouldFrontAttack();
                knight.Jump(knight.IsFrontAttack ? knight.smallJump : knight.bigJump, knight.IsFrontAttack);
            }
            else
            {
                knight.IsFrontAttack = true;
                knight.Jump(knight.bigJump, knight.IsFrontAttack);
            }
        }
        knight.PlayAnimation("Jump_FalseKnight");
    }

    public override void ExitState()
    {
        knight.IsAttacking = false;
        knight.Running = false;
    }

    public override void FrameUpdate()
    {
        if (knight.WasStunned)
        {
            if (Time.time - startTime > cooldownChangerState && knight.IsGrounded())
            {
                enemyStateMachine.ChangeState(knight.landState);
            }
        }
        else
        {
            if (Time.time - startTime > cooldownChangerState && knight.IsGrounded())
            {
                enemyStateMachine.ChangeState(knight.landState);
            }
            if (knight.IsAttacking && knight.IsFalling && knight.IsFrontAttack)
            {
                enemyStateMachine.ChangeState(knight.fallAttackState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
