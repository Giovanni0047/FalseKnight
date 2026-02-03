using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] damageEffectSpriteRenderer;
    private Color[] originalColor;
    [SerializeField]
    private Color damageColor;
    [SerializeField]
    private float damageEffectDuration = 0.1f;
    private void Start()
    {
        originalColor = new Color[damageEffectSpriteRenderer.Length];
        for (int i = 0; i < damageEffectSpriteRenderer.Length; i++)
        {
            originalColor[i] = damageEffectSpriteRenderer[i].color;
        }
    }
    public void ShowDamageEffect()
    {
        StartCoroutine(DamageEffectCoroutine());
    }
    private IEnumerator DamageEffectCoroutine()
    {
        foreach (var spriteRenderer in damageEffectSpriteRenderer)
        {
            spriteRenderer.color = damageColor;
        }
        yield return new WaitForSeconds(damageEffectDuration);
        for (int i = 0; i < damageEffectSpriteRenderer.Length; i++)
        {
            damageEffectSpriteRenderer[i].color = originalColor[i];
        }
    }
}
