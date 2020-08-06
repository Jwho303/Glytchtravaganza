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
	private int _shutdownFramerate = 15;
	private bool _hasFailed = false;
	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private TextMeshProUGUI _textmeshPro;
	[SerializeField]
	private CanvasGroup _canvasGroup;
	[SerializeField]
	private CanvasGroup _errorCanvasGroup;
	[SerializeField]
	private CanvasGroup _titleCanvasGroup;
	// Start is called before the first frame update
	void Awake()
	{
		Debug.LogFormat("[{0}] Starting Bootloader", this.name);
#if UNITY_WEBGL

		WebBrowser();
#else
		Desktop();
#endif
		//Application.targetFrameRate = 0;
		SceneManager.sceneLoaded += SceneLoaded;
		SceneManager.LoadScene("Main", LoadSceneMode.Additive);
		UIHelper.TurnOffCanvas(_canvasGroup);
		UIHelper.TurnOffCanvas(_errorCanvasGroup);
		UIHelper.TurnOffCanvas(_titleCanvasGroup);
		_camera.enabled = false;
		VideoController.Instance.SubscribeToVideoComplete(VideoComplete);
	}

	private void VideoComplete()
	{
		StartCoroutine(ShowTitle());
	}

	private IEnumerator ShowTitle()
	{
		_camera.enabled = true;
		UIHelper.TurnOnCanvas(_canvasGroup);
		UIHelper.TurnOnCanvas(_titleCanvasGroup);
		yield return new WaitForSecondsRealtime(5f);
		UIHelper.TurnOffCanvas(_canvasGroup);
		UIHelper.TurnOffCanvas(_titleCanvasGroup);
		_camera.enabled = false;
	}

	private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		SceneManager.SetActiveScene(arg0);
	}

	private void WebBrowser()
	{
		Application.targetFrameRate = -1;
		Debug.LogFormat("[{0}] Set framerate for webgl to {1}", this.name, Application.targetFrameRate);
	}

	private void Desktop()
	{
		Application.targetFrameRate = _targetFramerate;
		Debug.LogFormat("[{0}] Set framerate for desktop to {1}", this.name, Application.targetFrameRate);
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
		float framerate = Mathf.RoundToInt(1f / Time.deltaTime);

		if (framerate < _shutdownFramerate)
		{

			if (_shutDownTimer > _shutDownTime)
			{

				if (SceneManager.GetActiveScene() != this.gameObject.scene)
				{
					StopGame();
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

		if (_hasFailed)
		{
			#if !UNITY_WEBGL
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
			#endif
		}
	}

	[ContextMenu("Stop Game")]
	void StopGame()
	{
		_camera.enabled = true;
		SceneManager.SetActiveScene(this.gameObject.scene);
		SceneManager.UnloadSceneAsync("Main");
		_hasFailed = true;
		UIHelper.TurnOnCanvas(_canvasGroup);
		UIHelper.TurnOnCanvas(_errorCanvasGroup);
	}
}
