using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GlitchManager.GlitchIntensity glitchIntensity = GlitchManager.GlitchIntensity.Low;

    GlitchController GlitchController;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        GameController.Instance.RegisterManager(this);
        GameController.Instance.Init();
        ArtworkController.Instance.Init();
        GlitchController = GlitchController.Instance;
        GlitchController.Init();
    }

    private float _glitchFrequency = 2f;
    private float _lastGlitch = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (GlitchController.CanGlitch())
        {
            GlitchController.RandomGlitch(glitchIntensity);
        }
    }
}
