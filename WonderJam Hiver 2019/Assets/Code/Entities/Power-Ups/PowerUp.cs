using UnityEngine.UI;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public Image m_icon;

    public abstract void Use(Shooter p_shooter);
    public abstract void End(Shooter p_shooter);
}
