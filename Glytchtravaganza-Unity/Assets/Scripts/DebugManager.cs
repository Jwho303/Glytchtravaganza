using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
	GUIStyle debugStyle;
	private List<DebugEntry> debugEntries = new List<DebugEntry>();
	public void Awake()
	{
		debugStyle = new GUIStyle();
		debugStyle.fontSize = 40;
		debugStyle.fontStyle = FontStyle.Bold;
		debugStyle.normal.textColor = Color.white;
		BuildDebugMenu();

	}

	private void BuildDebugMenu()
	{

		debugEntries.Add(new DebugLabel("Frame Rate", () => { return Mathf.RoundToInt(1f / Time.deltaTime).ToString(); }));
		debugEntries.Add(new DebugLabel("Platform", () => { return Application.platform.ToString(); }));

	}

	public void OnGUI()
	{
		GUILayout.BeginVertical();
		foreach (var entry in debugEntries)
		{
			entry.Draw(debugStyle);
		}
		GUILayout.EndVertical();
	}

}

public abstract class DebugEntry
{
	public abstract void Draw(GUIStyle style);
}

public class DebugLabel : DebugEntry
{
	public string Name;
	public Func<string> Func;

	public DebugLabel(string name, Func<string> func)
	{
		Name = name;
		Func = func;
	}

	public DebugLabel(string name, string text)
	{
		Name = name;
		Func = () => { return text; };
	}

	public override void Draw(GUIStyle style)
	{
		String s = String.Format("{0} : {1}", Name, Func());
		GUILayout.Label(s, style);
	}
}
