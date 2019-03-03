using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Intensity")]
    public float m_duration = 0f;
    public float m_magnitude = 1f;

    [Header("Axis")]
    public bool m_x = true;
    public bool m_y = true;

    public void Shake()
    {
        StartCoroutine(Shake(m_duration, m_magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originePos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = (m_x) ? Random.Range(-m_magnitude, m_magnitude) : 0;
            float y = (m_y) ? Random.Range(-m_magnitude, m_magnitude) : 0;

            transform.localPosition =
                new Vector3(originePos.x + x, originePos.y + y, originePos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originePos;
    }
}
