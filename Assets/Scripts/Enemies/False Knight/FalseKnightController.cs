using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnightController : Enemy
{
    private Rigidbody2D rb2D;
    private Animator animator;
    [SerializeField]
    private DamageEffect damageEffect;

    [Header("Ray Ground")]
    [SerializeField]
    private float distanceRaycastGround;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Jump")]
    [SerializeField]
    internal Vector2 smallJump;
    [SerializeField]
    internal Vector2 bigJump;
    [SerializeField]
    internal float shortJumpDistance;
    [SerializeField]
    internal float longJumpDistance;
    [SerializeField]
    private Transform centerPoint;

    [Header("Move")]
    [SerializeField]
    private float runSpeed = 30;
    private bool running = false;

    //Flip
    private bool facingRight = false;
    private float directionToPlayer = - 1;
    private Transform playerTransform;

    #region State Machine
    private EnemyStateMachine enemyStateMachine;
    internal FalseKnightIdleState idleState;
    internal FalseKnightJumpAnticipate jumpAnticipate;
    internal FalseKnightJumpState jumpState;
    internal FalseKnightJumpLandState landState;
    internal FalseKnightFallAttackState fallAttackState;
    internal FalseKnightAttackState attackState;
    internal FalseKnightRunState runState;
    internal FalseKnightStunState stunState;
    internal FalseKnightStunEndState stunEndState;
    internal FalseKnightEnragedChargeState enragedChargeState;
    internal FalseKnightDeadState deadState;
    #endregion
    //Attack
    private bool isAttacking = false;
    private bool isFrontAttack = false;
    private bool animationFinished = false;
    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform positionSpawnProjectile;
    
    [Header("Stun")]
    [SerializeField]
    internal Vector2 jumpStun;
    private float percentage = 1.5f;
    private int countStun = 0;
    private bool wasStunned = false;


    private void Awake()
    {
        enemyStateMachine = new EnemyStateMachine();
        idleState = new FalseKnightIdleState(this, enemyStateMachine);
        jumpAnticipate = new FalseKnightJumpAnticipate(this, enemyStateMachine);
        jumpState = new FalseKnightJumpState(this, enemyStateMachine);
        landState = new FalseKnightJumpLandState(this, enemyStateMachine);
        fallAttackState = new FalseKnightFallAttackState(this, enemyStateMachine);
        attackState = new FalseKnightAttackState(this, enemyStateMachine);
        runState = new FalseKnightRunState(this, enemyStateMachine);
        stunState = new FalseKnightStunState(this, enemyStateMachine);
        stunEndState = new FalseKnightStunEndState(this, enemyStateMachine);
        enragedChargeState = new FalseKnightEnragedChargeState(this, enemyStateMachine);
        deadState = new FalseKnightDeadState(this, enemyStateMachine);
    }
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        enemyStateMachine.Initialize(idleState);
        
        CurrentHealth = MaxHealth;
    }
    private void Update()
    {
        enemyStateMachine.CurrentEnemyState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        enemyStateMachine.CurrentEnemyState.PhysicsUpdate();
    }
    public bool IsGrounded() => Physics2D.Raycast(transform.position, Vector2.down, distanceRaycastGround, groundLayer);
    public void StopMovement()
    {
        rb2D.velocity = Vector2.zero;
    }
    public void Movement()
    {
        rb2D.velocity = new Vector2(runSpeed * Time.fixedDeltaTime * directionToPlayer, rb2D.velocity.y);
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    public void FlipToPlayer()
    {
        if (playerTransform.position.x < transform.position.x && facingRight || playerTransform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        directionToPlayer *= -1;
        transform.localScale = localScale;
        facingRight = !facingRight;
    }
    public void Jump(Vector2 force,bool isFront)
    {
        if (isFront)
        {
            rb2D.AddForce(new Vector2(force.x * directionToPlayer, force.y), ForceMode2D.Impulse);
        }
        else
        {
            rb2D.AddForce(new Vector2(force.x * -directionToPlayer, force.y), ForceMode2D.Impulse);
        }

    }
    public enum AnimationTriggers
    {
        Projectile
    }
    public void AnimationTrigger(AnimationTriggers animationTriggers)
    {
        switch (animationTriggers)
        {
            case AnimationTriggers.Projectile:
                if (DistanceToPlayer() > shortJumpDistance)
                {
                    GameObject projectile = Instantiate(projectilePrefab, positionSpawnProjectile.position, Quaternion.identity);
                    projectile.GetComponent<GroundShockwave>().SetDirection(directionToPlayer);
                }
                break;
        }
    }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsFrontAttack { get => isFrontAttack; set => isFrontAttack = value; }
    public bool AnimationFinished { get => animationFinished; set => animationFinished = value; }
    public bool WasStunned { get => wasStunned; set => wasStunned = value; }
    public bool IsFalling { get => rb2D.velocity.y < 0; }
    public bool Running { get => running; set => running = value; }
    public void ShouldAttack()
    {
        IsAttacking =  Random.value < 0.75f;
    }
    public void ShouldFrontAttack()
    {
        IsFrontAttack = Random.value < 0.75f;
    }
    public void OnAnimationEnd()
    {
        AnimationFinished = true;
    }


    public float DistanceToPlayer() => Vector2.Distance(transform.position, playerTransform.position);
    public float DistanceToCenterPoint() => Vector2.Distance(transform.position, centerPoint.position);
    public bool CenterPointIsLeft() => centerPoint.position.x < transform.position.x;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanceRaycastGround);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        damageEffect.ShowDamageEffect();
        if (countStun < 3)
        {
            Stun(damage);
        }

    }
    public override void Die()
    {
        enemyStateMachine.ChangeState(deadState);
        GameManager.Instance.OnBossDead();
    }
    private void Stun(float damage)
    {
        float threshold = MaxHealth / percentage;

        if (CurrentHealth < threshold && CurrentHealth >= threshold - damage)
        {
            countStun++;
            percentage += 1.5f;
            enemyStateMachine.ChangeState(stunState);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            TakeDamage(2);
        }
    }
}
