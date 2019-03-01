using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour 
{
	public UnityEvent m_response;

	void Start() 
	{
		m_response.Invoke();
	}
}
