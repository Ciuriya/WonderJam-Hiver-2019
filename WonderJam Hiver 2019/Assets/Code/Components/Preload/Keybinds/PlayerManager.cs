using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour 
{
	[Tooltip("Prefabs matching the player's id to spawn")]
	public List<GameObject> m_playerPrefabs;

	public GameEvent m_gameStartTimerEvent;
    public GameEvent m_gameOverNoMorePlayers;

    public ValueManager m_lifeCountManager;

	[HideInInspector] public List<Player> m_players;
    public static int MostPlayers = 0;
    public static int GetMaxPlayers() { return MostPlayers; }

	void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene p_scene, LoadSceneMode p_mode)
	{
		if(p_scene.name != "DevTest") return;

		m_players = new List<Player>();

		foreach(InputUser user in Game.m_keybinds.GetAllActiveInputUsers(false))
			AddPlayer(user);
	}

	public bool AddPlayer(InputUser p_user) 
	{
		if(SceneManager.GetActiveScene().name != "DevTest") return false;
		if(m_players.Count == 4) return false;

		int id = 0;

		for(int i = 1; i <= 4; i++)
			if(!m_players.Exists(p => p.m_playerId == i))
			{
				id = i;
				break;
			}

		GameObject playerObj = Instantiate(m_playerPrefabs[id - 1]);
		Player player = playerObj.GetComponent<Player>();

		player.m_input = p_user;
		player.m_playerId = id;

		if(!player.Spawn()) 
		{
            Destroy(player.gameObject);
			return false;
		} 
		else m_players.Add(player);

		if(m_players.Count == 1)
			m_gameStartTimerEvent.Raise();

        if (id > MostPlayers)
            MostPlayers = id;

		return true;
	}

	public bool RemovePlayer(InputUser p_user, bool byDeath) 
	{
        if (!byDeath && m_players.Count == 1) return false;

		Player player = m_players.Find(p => p.m_input.m_profile == p_user.m_profile && p.m_input.m_controllerId == p_user.m_controllerId);

		if(player != null) 
		{         
			m_players.Remove(player);
            Game.m_keybinds.m_inputUsers.Remove(player.m_input);
            Destroy(player.gameObject);

            if (!byDeath) m_lifeCountManager.UpdateValue(1);

            if (m_players.Count == 0)
                m_gameOverNoMorePlayers.Raise();

            return true;
		}

		return false;
	}
}

