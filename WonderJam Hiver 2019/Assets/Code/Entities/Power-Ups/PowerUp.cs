using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public abstract void Use(Shooter p_shooter);
    public abstract void End(Shooter p_shooter);
}
