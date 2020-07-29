using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VideoData", menuName = "VideoData", order = 1)]
public class VideoData : ScriptableObject
{
	public List<VideoPath> videoPaths = new List<VideoPath>();

	public string GetVideoURL(string key)
	{
		return System.IO.Path.Combine(Application.streamingAssetsPath, videoPaths.Find(item => item.Key == key).FileName);
	}

	public VideoPath GetVideoPath(string key)
	{
		return videoPaths.Find(item => item.Key == key);
	}
}
[System.Serializable]
public class VideoPath
{
	public string Key;
	public string FileName;
	public bool LoadOnStart;
	public float JumpStartTime;
	public float JumpEndTime;

	public string VideoURL => System.IO.Path.Combine(Application.streamingAssetsPath, FileName);
	
}