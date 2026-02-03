using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShockwave : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField]
    private float shockwaveSpeed = 5f;
    private float direction = -1f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb2D.velocity = new Vector2(shockwaveSpeed * direction * Time.deltaTime, rb2D.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    public void SetDirection(float dir)
    {
        direction = dir;
    }
}
