using UnityEngine;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour 
{
	public List<Keybind> m_playerEntryKeys;
	public List<Keybind> m_mouseAndKeyboard;
	public List<Keybind> m_controller;

	[HideInInspector] public List<InputUser> m_inputUsers;
	[HideInInspector] public bool m_blockAllKeybinds;

	void OnEnable() 
	{
		m_inputUsers = new List<InputUser>();

		LoadKeybinds();

		foreach(Keybind keybind in GetAllKeybinds())
			keybind.m_axis.StartTracking();		

		SimpleInput.OnUpdate += OnKeybindUpdate;
	}

	void OnDisable() 
	{
		SaveKeybinds();

		foreach(Keybind keybind in GetAllKeybinds())
			keybind.m_axis.StopTracking();

		SimpleInput.OnUpdate -= OnKeybindUpdate;
	}

	public List<InputUser> GetAllActiveInputUsers() 
	{ 
		return m_inputUsers;
	}

	public List<Keybind> GetKeybinds(InputUser p_user) 
	{ 
		switch(p_user.m_profile) 
		{
			case "mouseAndKB": return m_mouseAndKeyboard;
			case "controller": return m_controller;
			default: return new List<Keybind>();
		}
	}

	public List<Keybind> GetAllKeybinds() 
	{ 
		List<Keybind> list = new List<Keybind>();

		list.AddRange(m_mouseAndKeyboard);

		return list;
	}

	private void LoadKeybinds() 
	{ 
		foreach(Keybind keybind in GetAllKeybinds())
		{ 
			string name = "Keybind-" + keybind.m_axis.Key.Replace(" ", "_");

			if(PlayerPrefs.HasKey(name + "Positive"))
				keybind.m_positiveKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name + "Positive"));

			if(PlayerPrefs.HasKey(name + "AltPositive"))
				keybind.m_altPositiveKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name + "AltPositive"));

			if(PlayerPrefs.HasKey(name + "Negative"))
				keybind.m_negativeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name + "Negative"));

			if(PlayerPrefs.HasKey(name + "AltNegative"))
				keybind.m_altNegativeKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name + "AltNegative"));
		}
	}

	private void SaveKeybinds() 
	{
		foreach(Keybind keybind in GetAllKeybinds()) SaveKeybind(keybind);
	}

	public void SaveKeybind(Keybind p_keybind) 
	{
		string name = "Keybind-" + p_keybind.m_axis.Key.Replace(" ", "_");

		PlayerPrefs.SetString(name + "Positive", p_keybind.m_positiveKey.ToString());
		PlayerPrefs.SetString(name + "AltPositive", p_keybind.m_altPositiveKey.ToString());
		PlayerPrefs.SetString(name + "Negative", p_keybind.m_negativeKey.ToString());
		PlayerPrefs.SetString(name + "AltNegative", p_keybind.m_altNegativeKey.ToString());
	}

	void Update() 
	{ 
		foreach(InputUser user in m_inputUsers) 
		{
			foreach(Keybind keybind in GetKeybinds(user))
			{
				// update runs right after OnKeybindUpdate, so we let it stay pressed/released until next frame
				if(keybind.m_pressedLastFrame)
				{
					keybind.m_pressedThisFrame = false;
					keybind.m_pressedLastFrame = false;
				}
				else if(keybind.m_pressedThisFrame) keybind.m_pressedLastFrame = true;

				if(keybind.m_releasedLastFrame)
				{
					keybind.m_releasedThisFrame = false;
					keybind.m_releasedLastFrame = false;
				}
				else if(keybind.m_releasedThisFrame) keybind.m_releasedLastFrame = true;
			}
		}
	}

	void OnKeybindUpdate() 
	{ 
		if(m_blockAllKeybinds) return;

		foreach(Keybind keybind in m_playerEntryKeys) 
		{
			InputUser user = new InputUser(keybind.m_profile, keybind.m_controllerSpecific ? keybind.m_controllerId : 0);
			bool exists = m_inputUsers.Exists(i => i.m_profile == user.m_profile && i.m_controllerId == user.m_controllerId);

			if(Input.GetKeyDown(keybind.m_positiveKey)) 
			{
				if(!exists && Game.m_players.AddPlayer(user)) m_inputUsers.Add(user);
			} 
			else if(Input.GetKeyDown(keybind.m_negativeKey)) 	
			{
				if(exists && Game.m_players.RemovePlayer(user)) 
					m_inputUsers.Remove(m_inputUsers.Find(i => i.m_profile == user.m_profile && i.m_controllerId == user.m_controllerId));
			}
		}

		foreach(InputUser user in m_inputUsers)
		{
			foreach(Keybind keybind in GetKeybinds(user))
			{
				if(Input.GetKey(keybind.m_negativeKey) || Input.GetKey(keybind.m_altNegativeKey))
					keybind.m_axis.value = -1f;
				else if(Input.GetKey(keybind.m_positiveKey) || Input.GetKey(keybind.m_altPositiveKey))
					keybind.m_axis.value = 1f;
				else keybind.m_axis.value = 0f;

				if(Input.GetKeyDown(keybind.m_negativeKey) || Input.GetKeyDown(keybind.m_altNegativeKey) ||
					Input.GetKeyDown(keybind.m_positiveKey) || Input.GetKeyDown(keybind.m_altPositiveKey))
					keybind.m_pressedThisFrame = true;

				if(Input.GetKeyUp(keybind.m_negativeKey) || Input.GetKeyUp(keybind.m_altNegativeKey) ||
					Input.GetKeyUp(keybind.m_positiveKey) || Input.GetKeyUp(keybind.m_altPositiveKey))
					keybind.m_releasedThisFrame = true;
			}
		}
	}

	public float GetAxis(string p_keybind, InputUser p_user) 
	{ 
		Keybind keybind = GetKeybinds(p_user).Find(k => k.m_axis.Key == p_keybind);
		
		if(keybind.m_useUnityAxis && p_user.m_profile == "controller")
			return keybind.m_useRawValues ? Input.GetAxisRaw(keybind.GetUpdatedUnityAxis(p_user.m_controllerId)) :
										    Input.GetAxis(keybind.GetUpdatedUnityAxis(p_user.m_controllerId));
		else return keybind.m_axis.value;
	}

	public bool GetButton(string p_keybind, InputUser p_user) 
	{
		Keybind keybind = GetKeybinds(p_user).Find(k => k.m_axis.Key == p_keybind);
		float axisValue = keybind.m_axis.value;

		if(keybind.m_useUnityAxis && p_user.m_profile == "controller")
			axisValue = keybind.m_useRawValues ? Input.GetAxisRaw(keybind.GetUpdatedUnityAxis(p_user.m_controllerId)) :
												 Input.GetAxis(keybind.GetUpdatedUnityAxis(p_user.m_controllerId));

		return axisValue == 1f;
	}

	public bool GetButtonDown(string p_keybind, InputUser p_user) 
	{ 
		return GetKeybinds(p_user).Find(k => k.m_axis.Key == p_keybind).m_pressedThisFrame;
	}	
	
	public bool GetButtonUp(string p_keybind, InputUser p_user) 
	{ 
		return GetKeybinds(p_user).Find(k => k.m_axis.Key == p_keybind).m_releasedThisFrame;
	}
}
