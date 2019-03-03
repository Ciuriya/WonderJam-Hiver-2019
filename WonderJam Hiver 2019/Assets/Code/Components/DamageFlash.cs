using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public float duration;

    public Color flashColor;

    public void Flash()
    {
        StartCoroutine(Flash(duration, GetComponent<SpriteRenderer>(), flashColor));
    }

    private IEnumerator Flash(float duration, SpriteRenderer renderer, Color color)
    {
        Color originColor;

        if (renderer != null)
        {
            originColor = renderer.color;
            renderer.color = color;
        }
        else
            yield break;

        float elapsed = 0;

        while ((elapsed += Time.deltaTime) < duration)
            yield return null;

        renderer.color = originColor;
    }
}
