using UnityEngine;
using System;

[Serializable]
public class InputUser 
{
	public string m_profile;
	public int m_controllerId;

	public InputUser(string p_profile, int p_controllerId) 
	{ 
		m_profile = p_profile;
		m_controllerId = p_controllerId;
	}
}
