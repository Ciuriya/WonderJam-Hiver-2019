using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OptionsMenuHandler : MonoBehaviour 
{
	[Tooltip("The dropdown handling the resolutions")]
	public Dropdown m_resolutionDropdown;

	[Tooltip("The toggle for vsync")]
	public Toggle m_vsyncToggle;

	[Tooltip("The slider handling the framerate")]
	public AdaptativeSliderText m_framerateSlider;

	[Tooltip("The dropdown handling the quality settings")]
	public Dropdown m_qualityDropdown;

	private int m_framerate;
	private bool m_fullscreen;
	private bool m_vsync;
	private int m_qualityLevel;
	private Resolution m_resolution;
	private Resolution[] m_resolutions;

	private OptionsMenuHandler() { }

	public static OptionsMenuHandler Instance { get; private set; }

	void Start()
	{
		Instance = this;

		m_qualityLevel = QualitySettings.GetQualityLevel();
		m_fullscreen = Screen.fullScreen;
		m_framerateSlider.m_unlimited = -1;
		m_qualityDropdown.value = m_qualityLevel;

		PopulateResolutions();
	}

	////////////////////////
	/*   Video Settings   */
	////////////////////////

	private void PopulateResolutions() 
	{
		m_resolutions = Screen.resolutions;
		List<Resolution> availableResolutions = new List<Resolution>();

		for(int i = 0; i < m_resolutions.Length; i++) 
		{
			if(Screen.currentResolution.refreshRate != m_resolutions[i].refreshRate) continue;

			Dropdown.OptionData data = new Dropdown.OptionData(ResolutionToString(m_resolutions[i]));
			m_resolutionDropdown.options.Add(data);

			availableResolutions.Add(m_resolutions[i]);

			if(!Screen.fullScreen) 
			{ 
				if(Screen.width == m_resolutions[i].width && Screen.height == m_resolutions[i].height) 
				{
					m_resolutionDropdown.value = i;
					m_resolution = m_resolutions[i];
				}
			} 
			else if(Screen.currentResolution.Equals(m_resolutions[i])) 
			{
				m_resolutionDropdown.value = i;
				m_resolution = m_resolutions[i];
			}
		}

		m_resolutions = new Resolution[availableResolutions.Count];

		for(int i = 0; i < availableResolutions.Count; i++) 
			m_resolutions[i] = availableResolutions[i];
	}

	private IEnumerator UpdateResolution() 
	{ 
		yield return new WaitForSeconds(1f);

		m_resolutionDropdown.ClearOptions();
		PopulateResolutions();
	}

	public void ApplyOptions() 
	{
		SetResolution();
		ApplyVSync();
		ApplyFramerate();

		ApplyResolution();
		QualitySettings.SetQualityLevel(m_qualityLevel, true);

		StartCoroutine(UpdateResolution());
	}

	public void SetResolution() 
	{ 
		m_resolution = m_resolutions[m_resolutionDropdown.value];
	}

	public void ApplyResolution() 
	{
		if(m_resolution.width == m_resolutions[m_resolutions.Length - 1].width && !m_fullscreen)
			m_fullscreen = true;
		else if(m_resolution.width != m_resolutions[m_resolutions.Length - 1].width && m_fullscreen)
			m_fullscreen = false;

		ApplyResolution(m_resolution.width,
						   m_resolution.height,
						   m_fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed,
						   m_resolution.refreshRate);
	}

	public void ApplyResolution(int p_width, int p_height, FullScreenMode p_fullscreen, int p_refreshRate)
	{
		Screen.SetResolution(p_width,
								 p_height,
								 p_fullscreen,
								 p_refreshRate);
	}

	public void SetVSync(bool p_vsync) 
	{ 
		m_vsync = p_vsync;
	}

	public void ApplyVSync() 
	{ 
		QualitySettings.vSyncCount = m_vsync ? 1 : 0;
	}

	public void SetFramerate(int p_framerate)
	{
		m_framerate = p_framerate;

		if(m_framerateSlider.m_value != p_framerate)
			m_framerateSlider.m_value = p_framerate;
	}

	public void ApplyFramerate() 
	{ 
		Application.targetFrameRate = m_framerate;
	}

	public string ResolutionToString(Resolution p_resolution) 
	{ 
		return p_resolution.width + "x" + p_resolution.height;
	}

	public void SetQualitySettings(int p_quality) 
	{ 
		m_qualityLevel = p_quality;
	}

	public void SetBrightness(int p_brightness) 
	{ 
		RenderSettings.ambientLight = new Color(p_brightness / 100f, p_brightness / 100f, p_brightness / 100f, 1);
	}

	////////////////////////
	/*    Audio Settings  */
	////////////////////////

	public void SetMasterVolume(int p_volume)
	{ 
		AudioListener.volume = (float) p_volume / 100f;
	}

	/////////////////////////////
	/*    Gameplay Settings    */
	/////////////////////////////

	////////////////////////////
	/*   Controls Settings    */
	////////////////////////////
	
	
}
