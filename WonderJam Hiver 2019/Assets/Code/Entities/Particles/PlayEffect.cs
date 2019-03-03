using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<ParticleSystem>().isPlaying)
            Destroy(gameObject);
    }
}
