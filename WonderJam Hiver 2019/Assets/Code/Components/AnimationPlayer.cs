using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();

    }

    public void playAnimation()
    {
        m_animator.SetTrigger("PlayAnimation");
    }
}
