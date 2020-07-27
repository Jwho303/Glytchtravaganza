using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	GlitchIntensity glitchIntensity = GlitchIntensity.Low;

	GlitchController GlitchController;
	// Start is called before the first frame update

	private void Awake()
	{
#if UNITY_EDITOR
		Desktop();
#elif UNITY_WEBGL
		WebBrowser();
#else
		Desktop();
#endif
		GameController.Instance.RegisterManager(this);
		GameController.Instance.Init();
	}

	private void WebBrowser()
	{
		Application.targetFrameRate = -1;

	}

	private void Desktop()
	{
		Application.targetFrameRate = 60;
	}

	void Start()
	{
		ArtworkController.Instance.Init();
		AudioController.Instance.Init();
		GlitchController = GlitchController.Instance;
		GlitchController.Init();
		VideoController.Instance.Init();

		GlitchController.Instance.SetGlitchIntensity(GlitchIntensity.None);
	}

	// Update is called once per frame
	void Update()
	{
		glitchIntensity = GlitchController.Instance.GlitchIntensity;

		if (GlitchController.CanGlitch())
		{
			GlitchController.RandomGlitch();
		}
	}

	private void OnValidate()
	{
		GlitchController.Instance.SetGlitchIntensity(glitchIntensity);
	}
}
