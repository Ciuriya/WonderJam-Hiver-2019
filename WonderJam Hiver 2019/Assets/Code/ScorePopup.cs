using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(PopUp());
    }

    private void Update()
    {
        float speed = 90f;
        float newY = Time.deltaTime * speed;
        transform.Translate(0, newY, 0);
    }

    IEnumerator PopUp()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }

}
