using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        GameController.Instance.RegisterManager(this);
        GameController.Instance.Init();
        ArtworkController.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
