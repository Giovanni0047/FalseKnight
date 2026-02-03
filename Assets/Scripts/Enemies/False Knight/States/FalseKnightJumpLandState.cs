
public class FalseKnightJumpLandState : EnemyState
{
    FalseKnightController knight;
    public FalseKnightJumpLandState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("Jump(Land)_FalseKnight");
        knight.StopMovement();
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
            if (knight.WasStunned)
            {
                enemyStateMachine.ChangeState(knight.enragedChargeState);
            }
            else
            {
                if (knight.IsFrontAttack)
                {
                    enemyStateMachine.ChangeState(knight.idleState);
                }
                else
                {
                    enemyStateMachine.ChangeState(knight.attackState);
                }
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
