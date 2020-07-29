using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlitchData", menuName = "GlitchData", order = 1)]
public class GlitchData : ScriptableObject
{
	public List<GlitchSettings> GlitchSettings = new List<GlitchSettings>();
}

[System.Serializable]
public class GlitchSettings
{
	public GlitchIntensity GlitchIntensity;
	public float GlitchFrequency = 2f;
	public float ScreenGlitchDuration = 0.3f;
	public float ObjectGlitchDuration = 10f;
	public int GlitchObjectCount = 5;
	public bool ReverseNormals = false;
	public string SoundKey;
	public bool PlayVideo;
	public string[] VideoKeys;
	public bool JumpVideo;
	public float VideoDuration;

	public string VideoKey => VideoKeys[Random.Range(0, VideoKeys.Length)];
}