using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    public void playAnimation()
    {
        m_animator.SetTrigger("PlayAnimation");
    }
}
