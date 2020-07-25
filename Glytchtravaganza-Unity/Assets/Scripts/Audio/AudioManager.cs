using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	
	private void Awake()
	{
		AudioController.Instance.RegisterManager(this);
	}

	internal void SpawnClips(AudioData audioData)
	{
		
	}

	internal void PlayClip(string key)
	{
		
	}
}
