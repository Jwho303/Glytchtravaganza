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
	private float _glitchFrequency = 2f;
	private float _lastGlitch = 0f;
	private float _glitchDuration = 0.3f;

	private Action<float> _digitalGlitch;
	private Action<float> _analogueGlitch;
	private Action<float> _normalsGlitch;
	private Action<float> _vertexGlitch;
	private Action<float> _resetAction;

	private Coroutine _glitchCoroutine;



	private void Awake()
	{
		_digitalGlitch = (value) =>
		{
			digitalGlitch.intensity = Mathf.Lerp(0f, 0.25f, value);
		};

		_analogueGlitch = (value) =>
		{
			analogGlitch.scanLineJitter = Mathf.Lerp(0f, 0.5f, value);
			analogGlitch.colorDrift = Mathf.Lerp(0f, 0.5f, value);
		};

		_glitchObjects = FindObjectsOfType<GameObjectGlitch>().ToList();

		_normalsGlitch = (value) =>
		{
			int index = Mathf.RoundToInt(Mathf.Lerp(-1f, ((float)_glitchObjects.Count - 1), value));
			if (index >= 0)
			{
				for (int i = 0; i < _glitchObjects.Count; i++)
				{
					if (i == index)
					{
						_glitchObjects[i].ReverseNormals();
					}
					else
					{
						_glitchObjects[i].ResetMesh();
					}
				}
			}
			else
			{
				for (int i = 0; i < _glitchObjects.Count; i++)
				{
					_glitchObjects[i].ResetMesh();
				}
			}
		};

		_vertexGlitch = (value) =>
		{
			int index = Mathf.RoundToInt(Mathf.Lerp(-1f, ((float)_glitchObjects.Count - 1), value));
			if (index >= 0)
			{
				for (int i = 0; i < _glitchObjects.Count; i++)
				{
					if (i == index)
					{
						_glitchObjects[i].RandomizeVerts();
					}
					else
					{
						_glitchObjects[i].ResetMesh();
					}
				}
			}
			else
			{
				for (int i = 0; i < _glitchObjects.Count; i++)
				{
					_glitchObjects[i].ResetMesh();
				}
			}
		};

		_resetAction = (value) =>
		{
			Debug.Log("RESET!");
			for (int i = 0; i < _glitchObjects.Count; i++)
			{
				_glitchObjects[i].ResetMesh();
			}
		};
	}


	private void Update()
	{
		if (Time.unscaledTime >= (_lastGlitch + _glitchFrequency) && _glitchCoroutine == null && _glitchOnTime)
		{
		//	RandomGlitch();
		}
	}

	[ContextMenu("Reset Glitch Objects")]
	public void ResetGlitchObjects()
	{
		for (int i = 0; i < _glitchObjects.Count; i++)
		{
			_glitchObjects[i].ResetMesh();
		}
	}

	private void RandomGlitch()
	{
		Debug.Log("Glitch!");
		_lastGlitch = Time.unscaledTime;

		int _randomGlitch = (UnityEngine.Random.Range(0, 4));
		IEnumerator selectedAction = null;

		switch (_randomGlitch)
		{
			case 0:
				selectedAction = FadeGlitch(_digitalGlitch, _glitchDuration);
				break;
			case 1:
				selectedAction = FadeGlitch(_analogueGlitch, _glitchDuration);
				break;
			case 2:
				selectedAction = FadeGlitch(_normalsGlitch, _glitchDuration * 2f);
				break;
			case 3:
				selectedAction = FadeGlitch(_vertexGlitch, _glitchDuration * 2f);
				break;
			case 4:
				//selectedAction = FadeGlitch(_resetAction, _glitchDuration * 2f);
				break;

		}

		StartCoroutine(selectedAction);
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
		_glitchCoroutine = null;
	}

}
