using Kino;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlitchManager : MonoBehaviour
{
	[SerializeField]
	private AnalogGlitch analogGlitch;
	[SerializeField]
	private DigitalGlitch digitalGlitch;

	List<GameObjectGlitch> _glitchObjects = new List<GameObjectGlitch>();
	private GameObjectGlitch RandomGlitchObject
	{ get { return (_glitchObjects[UnityEngine.Random.Range(0, _glitchObjects.Count - 1)]); } }

	[SerializeField]
	private bool _glitchOnTime = true;
	private float _lastGlitch = 0f;

	private Action<float> _digitalGlitch => (value) => DigitalScreenGlitch(value);
	private Action<float> _analogueGlitch => (value) => AnalogueScreenGlitch(value);
	private Action<float> _screenGlitch => (UnityEngine.Random.Range(0, 2) < 1) ? _digitalGlitch : _analogueGlitch;

	private Coroutine _glitchCoroutine;
	private Coroutine _screenGlitchCoroutine;

	private void DigitalScreenGlitch(float value)
	{
		if (value == 0f && digitalGlitch.enabled)
		{
			digitalGlitch.enabled = false;
		}
		else if (value != 0f && !digitalGlitch.enabled)
		{
			digitalGlitch.enabled = true;
		}

		digitalGlitch.intensity = Mathf.Lerp(0f, 0.1f, value);
	}

	private void AnalogueScreenGlitch(float value)
	{
		if (value == 0f && analogGlitch.enabled)
		{
			analogGlitch.enabled = false;
		}
		else if (value != 0f && !analogGlitch.enabled)
		{
			analogGlitch.enabled = true;
		}

		analogGlitch.scanLineJitter = Mathf.Lerp(0f, 0.75f, value);
		analogGlitch.colorDrift = Mathf.Lerp(0f, 0.5f, value);
	}

	private void GlitchOutObject(GameObjectGlitch gameObjectGlitch)
	{
		gameObjectGlitch.IsGLitched = true;
	}

	private void ResetGlitchObject(GameObjectGlitch gameObjectGlitch)
	{
		gameObjectGlitch.IsGLitched = false;
	}

	internal void StopGlitches()
	{
		if (_screenGlitchCoroutine != null)
		{
			StopCoroutine(_screenGlitchCoroutine);
			DigitalScreenGlitch(0f);
			_screenGlitchCoroutine = null;
		}

		if (_glitchCoroutine != null)
		{
			StopCoroutine(_glitchCoroutine);
			_glitchCoroutine = null;
		}

		ResetAllGlitches();
	}

	[ContextMenu("Reset Glitch Objects")]
	private void ResetAllGlitches()
	{
		for (int i = 0; i < _glitchObjects.Count; i++)
		{
			_glitchObjects[i].IsGLitched = false;
		}
	}

	private void Awake()
	{
		GlitchController.Instance.RegisterManager(this);
		_glitchObjects = FindObjectsOfType<GameObjectGlitch>().ToList();
		DigitalScreenGlitch(0f);
		AnalogueScreenGlitch(0f);
	}

	public bool CanGlitch()
	{
		return Time.unscaledTime >= (_lastGlitch + GlitchController.Instance.Settings.GlitchFrequency) && _glitchCoroutine == null && _glitchOnTime;
	}

	private void Update()
	{

	}

	public void RandomGlitch(GlitchIntensity glitchIntensity)
	{
		if (glitchIntensity != GlitchIntensity.None)
		{
			//Debug.Log("Glitch!");
			_lastGlitch = Time.unscaledTime;
			_glitchCoroutine = StartCoroutine(GlitchOut(glitchIntensity));
		}
	}

	private IEnumerator FadeGlitch(Action<float> glitchAction, float duration)
	{
		float startTime = Time.unscaledTime;
		while (Time.unscaledTime <= startTime + duration)
		{
			float t = Mathf.Clamp01((Time.unscaledTime - startTime) / (duration));
			glitchAction(1f - t);
			yield return new WaitForEndOfFrame();
		}

		glitchAction(0f);
	}

	private IEnumerator GlitchOut(GlitchIntensity intensity)
	{

		float startTime = Time.unscaledTime;

		StartCoroutine(FadeGlitch(_analogueGlitch, GlitchController.Instance.Settings.ScreenGlitchDuration));

		if (_screenGlitchCoroutine != null)
		{
			StopCoroutine(_screenGlitchCoroutine);
		}
		_screenGlitchCoroutine = StartCoroutine(FadeGlitch(_digitalGlitch, GlitchController.Instance.Settings.ObjectGlitchDuration));
		//Debug.Log("Start Co");
		List<GameObjectGlitch> glitchObjects = new List<GameObjectGlitch>();

		for (int i = 0; i < GlitchController.Instance.Settings.GlitchObjectCount; i++)
		{
			GameObjectGlitch gameObjectGlitch = RandomGlitchObject;
			//Debug.Log(gameObjectGlitch.name);
			GlitchOutObject(gameObjectGlitch);
			glitchObjects.Add(gameObjectGlitch);
		}

		while (Time.unscaledTime <= startTime + GlitchController.Instance.Settings.ObjectGlitchDuration)
		{
			yield return new WaitForEndOfFrame();
		}

		for (int i = 0; i < glitchObjects.Count; i++)
		{
			ResetGlitchObject(glitchObjects[i]);
		}
		//Debug.Log("End Co");
		_glitchCoroutine = null;
	}

}

