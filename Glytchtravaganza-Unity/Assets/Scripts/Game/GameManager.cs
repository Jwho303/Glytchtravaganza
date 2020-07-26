using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GlitchIntensity glitchIntensity = GlitchIntensity.Low;

    GlitchController GlitchController;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        GameController.Instance.RegisterManager(this);
        GameController.Instance.Init();
        ArtworkController.Instance.Init();
        AudioController.Instance.Init();
        GlitchController = GlitchController.Instance;
        GlitchController.Init();

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
