using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchListener : MonoBehaviour
{
	[SerializeField]
	private GlitchIntensity[] _onIntensity;

	protected bool _isOn = true;
	protected Action _onAction = delegate { };
	protected Action _offAction = delegate { };
	protected Action _openArtworkAction = delegate { };
	protected Action _closeArtworkAction = delegate { };
	// Start is called before the first frame update
	public virtual void Start()
	{
		GlitchController.Instance.SubscribeToGlitchIntensityChange(GlitchIntensityChange);
		ArtworkController.Instance.SubscribeToOpenGallery(OpenArtWork);
		ArtworkController.Instance.SubscribeToCloseGallery(CloseArtWork);
	}

	private void CloseArtWork()
	{
		_closeArtworkAction();
	}

	private void OpenArtWork(Artwork artwork)
	{
		_openArtworkAction();
	}

	private void GlitchIntensityChange(GlitchIntensity intensity)
	{
		bool shouldBeOn = false;

		for (int i = 0; i < _onIntensity.Length; i++)
		{
			if (intensity == _onIntensity[i])
			{
				shouldBeOn = true;
				break;
			}
		}

		if (!_isOn && shouldBeOn)
		{
			_isOn = true;
			_onAction();
		}
		else if (_isOn && !shouldBeOn)
		{
			_isOn =false;
			_offAction();
		}
	}
}
