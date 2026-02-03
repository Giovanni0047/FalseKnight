using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool playerDead = false;
    private bool bossDead = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerDead()
    {
        playerDead = true;
        DisablePlayerEnemyCollision();
    }
    public void OnBossDead()
    {
        DisablePlayerEnemyCollision();
        bossDead = true;
    }
    private void DisablePlayerEnemyCollision()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );
    }
    public bool IsPlayerDead
    {
        get => playerDead;
    }
    public bool IsBossDead
    {
        get => bossDead;
    }
}
