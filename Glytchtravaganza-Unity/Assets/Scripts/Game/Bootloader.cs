using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Bootloader : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private float _shutDownTimer = 0f;
	private float _shutDownTime = 5f;
	private int _targetFramerate = 60;
	private bool _hasFailed = false;
	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private TextMeshProUGUI _textmeshPro;
	[SerializeField]
	private CanvasGroup _canvasGroup;
	// Start is called before the first frame update
	void Awake()
	{
#if UNITY_EDITOR
		Desktop();
#elif UNITY_WEBGL
		WebBrowser();
#else
		Desktop();
#endif
		SceneManager.sceneLoaded += SceneLoaded;
		SceneManager.LoadScene("Main", LoadSceneMode.Additive);
		UIHelper.TurnOffCanvas(_canvasGroup);
	}

	private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		SceneManager.SetActiveScene(arg0);
	}

	private void WebBrowser()
	{
		Application.targetFrameRate = _targetFramerate;

	}

	private void Desktop()
	{
		Application.targetFrameRate = _targetFramerate;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_hasFailed)
		{
			Debug.Log("Clicking");
				int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textmeshPro, Input.mousePosition, _camera);
				if (linkIndex != -1)
				{ // was a link clicked?
					TMP_LinkInfo linkInfo = _textmeshPro.textInfo.linkInfo[linkIndex];

					// open the link id as a url, which is the metadata we added in the text field
					Application.OpenURL(linkInfo.GetLinkID());
				}

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Mathf.RoundToInt(1f / Time.deltaTime) < (_targetFramerate - 10))
		{

			if (_shutDownTimer > _shutDownTime)
			{

				if (_targetFramerate == 60)
				{
					_shutDownTimer = 0;
					_targetFramerate = 30;
					Application.targetFrameRate = -1;
				}
				else
				{
					if (SceneManager.GetActiveScene() != this.gameObject.scene)
					{
						StopGame();
					}
				}
			}
			else
			{
				_shutDownTimer += Time.deltaTime;
			}
		}
		else
		{
			_shutDownTimer = 0f;

		}
	}

	[ContextMenu("Stop Game")]
	void StopGame()
	{
		SceneManager.SetActiveScene(this.gameObject.scene);
		SceneManager.UnloadSceneAsync("Main");
		_hasFailed = true;
		UIHelper.TurnOnCanvas(_canvasGroup);
	}
}
