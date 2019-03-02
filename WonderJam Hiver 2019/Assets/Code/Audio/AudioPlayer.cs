using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	[Tooltip("Audio event that will be played on start")]
	public SimpleAudioEvent m_audioEvent;

	[Tooltip("Should the audio loop?")]
	public bool m_loop;

	private AudioSource m_audioSource;

	public void Start()
	{
		m_audioSource = gameObject.AddComponent<AudioSource>();
		m_audioSource.loop = true;

		Game.m_audio.AddAudioSource(m_audioSource, AudioCategories.Music);
		m_audioEvent.Play(m_audioSource);
	}
}
