using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour 
{
	[Tooltip("The prefab to instantiate for each player")]
	public GameObject m_playerPrefab;

	[HideInInspector] public List<Player> m_players;

	void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene p_scene, LoadSceneMode p_mode)
	{
		if(p_scene.name != "DevTest") return;

		m_players = new List<Player>();

		foreach(InputUser user in Game.m_keybinds.GetAllActiveInputUsers())
			AddPlayer(user);
	}

	public bool AddPlayer(InputUser p_user) 
	{ 
		if(m_players.Count == 4) return false;

		GameObject playerObj = Instantiate(m_playerPrefab);
		Player player = playerObj.GetComponent<Player>();

		player.m_input = p_user;
		
		for(int i = 1; i <= 4; i++)
			if(!m_players.Exists(p => p.m_playerId == i)) {
				player.m_playerId = i;
				break;
			}

		if(!player.Spawn()) 
		{
			player.Despawn();
			return false;
		} 
		else m_players.Add(player);

		return true;
	}

	public bool RemovePlayer(InputUser p_user) 
	{
		if(m_players.Count == 1) return false;

		Player player = m_players.Find(p => p.m_input.m_profile == p_user.m_profile && p.m_input.m_controllerId == p_user.m_controllerId);

		if(player != null) 
		{
			m_players.Remove(player);
			player.Despawn();

			return true;
		}

		return false;
	}
}

