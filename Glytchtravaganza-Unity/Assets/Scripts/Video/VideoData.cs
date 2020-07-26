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
}
[System.Serializable]
public class VideoPath
{
	public string Key;
	public string FileName;
}