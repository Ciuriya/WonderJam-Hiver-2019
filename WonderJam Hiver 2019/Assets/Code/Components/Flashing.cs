using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    public float duration;
    public float flashSpeed;

    public Color flashColor;

    public void Flash()
    {
        StartCoroutine(Flash(duration, flashSpeed, GetComponent<SpriteRenderer>(), flashColor));
    }

    private IEnumerator Flash(float duration, float flashSpeed, SpriteRenderer renderer, Color flashingColor)
    {
        Color originColor;

        if (renderer != null)
            originColor = renderer.color;
        else
            yield break;

        float elapsed = 0;
        bool isON = true;


        while (elapsed < duration)
        {
            float colorElapse = 0;
            renderer.color = (isON) ? flashingColor : originColor;

            while((colorElapse += Time.deltaTime) < flashSpeed)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            isON = !isON;
        }

        renderer.color = originColor;
    }
}
