
public class FalseKnightRunState : EnemyState
{
    private FalseKnightController knight;
    public FalseKnightRunState(FalseKnightController enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        this.knight = enemy;
    }

    public override void EnterState()
    {
        knight.PlayAnimation("Run_FalseKnight");
        knight.IsAttacking = true;
        knight.Running = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (knight.DistanceToPlayer() < knight.longJumpDistance)
        {
            enemyStateMachine.ChangeState(knight.jumpAnticipate);
        }
    }

    public override void PhysicsUpdate()
    {
        knight.Movement();
    }
}
