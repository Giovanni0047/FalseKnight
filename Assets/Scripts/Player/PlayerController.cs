using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,IDamageable
{
    private Rigidbody2D rb2D;
    private Animator animator;
    private PlayerInput playerInput;
    [SerializeField]
    private DamageEffect damageEffect;

    private float moveInputX;
    private float moveInputY;

    [Header("Movement")]
    [SerializeField]
    private float speed = 5f;

    [Header("Jump")]
    [SerializeField]
    private float forceJump = 10f;

    [Header("Ray Ground")]
    [SerializeField]
    private float rayGroundLength = 0.1f;
    private bool isGrounded;

    //Flip
    private bool facingRight = true;

    //Attack
    private float cooldownAttack;

    [field:SerializeField]public float MaxHealth { get; set; } = 4f;
    public float CurrentHealth { get; set; }

    //Invincibility
    private bool invincible = false;
    private float invincibilityDuration = 1f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        cooldownAttack = Time.time;
        CurrentHealth = MaxHealth;
    }
    private void Update()
    {
        //Input
        if (GameManager.Instance.IsPlayerDead == false)
        {
            moveInputX = playerInput.actions["Move"].ReadValue<Vector2>().x;
            moveInputY = playerInput.actions["Move"].ReadValue<Vector2>().y;
        }
        else { rb2D.velocity = Vector2.zero; }

        //Animations
        animator.SetFloat("Move", Mathf.Abs(moveInputX));
        animator.SetFloat("Fall",rb2D.velocity.y);
        animator.SetBool("HitGround", isGrounded);

        isGrounded = HitGrounded();
        FLip(moveInputX);
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPlayerDead == false)
        {
            Movement();
        }
    }

    private void Movement()
    {
        rb2D.velocity = new Vector2(moveInputX * speed * Time.fixedDeltaTime, rb2D.velocity.y);
    }
    private bool HitGrounded() => Physics2D.Raycast(transform.position, Vector2.down, rayGroundLength, LayerMask.GetMask("Ground"));
    private void FLip(float input)
    {
        if (facingRight && input < 0 || !facingRight && input > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            facingRight = !facingRight;
        }
    }
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && GameManager.Instance.IsPlayerDead == false)
        {
            if (isGrounded)
            {
                animator.SetTrigger("Jump");
                rb2D.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
            }
        }
    }
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && Time.time - cooldownAttack > 0.5f && GameManager.Instance.IsPlayerDead == false)
        {
            cooldownAttack = Time.time;
            if (moveInputY > 0)
            {
                animator.SetTrigger("AttackUp");
            }
            else if (moveInputY < 0 && !isGrounded)
            {
                animator.SetTrigger("AttackDown");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down* rayGroundLength);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !invincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    private IEnumerator InvincibilityCoroutine()
    {
        invincible = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        TakeDamage(1);
        yield return new WaitForSeconds(invincibilityDuration);
        invincible = false;
        if (GameManager.Instance.IsPlayerDead == false)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
    }
    public void TakeDamage(int damage)
    {
        UIManager.Instance.TakeDamage();
        damageEffect.ShowDamageEffect();
        CurrentHealth -= damage;
        if (CurrentHealth <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetTrigger("Die");
        GameManager.Instance.OnPlayerDead();
    }
}
