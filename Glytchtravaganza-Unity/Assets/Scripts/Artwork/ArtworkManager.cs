using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ArtworkController.Instance.RegisterManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal void ArtworkSelected(ArtworkClickable artworkClickable)
	{
        Debug.Log(artworkClickable.Name);
	}
}
