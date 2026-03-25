using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer childSprite;

    private bool isReady = true;
    private float cooldownTime = 60f;

    public bool IsReady() => isReady;

    public void ResetToBlack()
    {
        childSprite.color = Color.black;
        isReady = true;
    }

    public void Activate()
    {
        if (!isReady) return;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isReady = false;

        float timer = 0f;

        childSprite.color = Color.black;

        while (timer < cooldownTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / cooldownTime;

            childSprite.color = Color.Lerp(Color.black, Color.white, t);

            yield return null;
        }

        childSprite.color = Color.white;
        isReady = true;
    }
}
