public class FalseKnightJumpAnticipate : EnemyState
{
    FalseKnightController knight;
    public FalseKnightJumpAnticipate(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.knight = enemy;
    }
    public override void EnterState()
    {
        knight.StopMovement();
        if (!knight.IsAttacking) { knight.ShouldAttack(); }
        knight.PlayAnimation("Jump(Anticipate)_FalseKnight");
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
            enemyStateMachine.ChangeState(knight.jumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
