using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private bool _isDragging = false;
	private bool _keyDown = false;
	[SerializeField]
	private Vector2 _dragOrigin = Vector2.zero;
	public Vector2 _direction = Vector2.zero;
	public Vector2 _magnitude = Vector2.zero;
	private float _maxMagnitude = 50f;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CanvasGroup _interactionCanvasGroup;

	private ArtworkClickable _selectedClickable = null;

	private void Awake()
	{
		ArtworkController.Instance.SubscribeToOpenGallery(GalleryOpen);
		ArtworkController.Instance.SubscribeToCloseGallery(GalleryClose);

		InputController.Instance.SubscribeToArtworkMissed(HideInteractionButton);
		InputController.Instance.SubscribeToArtworkHit(ShowInteractionButton);

		GalleryClose();
	}

	private void ShowInteractionButton(ArtworkClickable obj)
	{
		UIHelper.TurnOnCanvas(_interactionCanvasGroup);
		_selectedClickable = obj;
	}

	private void HideInteractionButton()
	{
		UIHelper.TurnOffCanvas(_interactionCanvasGroup);
		_selectedClickable = null;
	}

	private void GalleryClose()
	{
		UIHelper.TurnOnCanvas(_canvasGroup);
	}

	private void GalleryOpen(Artwork artwork)
	{
		UIHelper.TurnOffCanvas(_canvasGroup);
	}

	public void FixedUpdate()
	{
		if (ArtworkController.Instance.IsOpen)
		{
			GalleryControls();
		}
		else
		{
			CheckKeyPresses();

			if (_isDragging || _keyDown)
			{
				InputController.Instance.Translate(_direction.y, _magnitude.y);
				InputController.Instance.Rotate(_direction.x, _magnitude.x);
			}
		}
	}

	private void GalleryControls()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire1"))
		{
			ArtworkController.Instance.ArtworkClosed();
		}

		if (Input.GetAxis("Horizontal") > 0.5f && !_keyDown)
		{
			ArtworkController.Instance.NextItem();
			KeyDown();
		}
		else if (Input.GetAxis("Horizontal") < -0.5f && !_keyDown)
		{
			ArtworkController.Instance.PreviousItem();
			KeyDown();
		}
		else if (Input.GetAxis("Horizontal") > -0.5f && Input.GetAxis("Horizontal") < 0.5f && _keyDown)
		{
			KeyUp();
		}

	}

	private void CheckKeyPresses()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (_keyDown && !Input.anyKey)
		{
			KeyUp();
			//Debug.LogFormat("[{0}] Key Up", this.name);
		}

		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			KeyDown();
			//Debug.LogFormat("[{0}] Key Down", this.name);
			_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			_magnitude = Vector2.one;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Interaction();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.LogFormat("[{0}] Begin Dragging", this.name);
		_isDragging = true;
		_dragOrigin = eventData.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		//Debug.LogFormat("[{0}] On Dragging", this.name);
		_direction = (eventData.position - _dragOrigin).normalized;
		_magnitude.x = Mathf.Clamp(Mathf.Abs(eventData.position.x - _dragOrigin.x), 0f, _maxMagnitude) / _maxMagnitude;
		_magnitude.y = Mathf.Clamp(Mathf.Abs(eventData.position.y - _dragOrigin.y), 0f, _maxMagnitude) / _maxMagnitude;

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//Debug.LogFormat("[{0}] End Dragging", this.name);
		_isDragging = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_isDragging)
		{
			//Debug.LogFormat("[{0}] Pointer Click", this.name);
			InputController.Instance.Tap(eventData.position);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		ClearData();
	}

	private void ClearData()
	{
		_dragOrigin = Vector2.zero;
		_direction = Vector2.zero;
		_magnitude = Vector2.zero;
	}

	public void Up()
	{
		KeyDown();
		_direction = Vector2.up;
		_magnitude = Vector2.one;
	}

	public void Down()
	{
		KeyDown();
		_direction = Vector2.down;
		_magnitude = Vector2.one;
	}

	public void Left()
	{
		KeyDown();
		_direction = Vector2.left;
		_magnitude = Vector2.one;
	}

	public void Right()
	{
		KeyDown();
		_direction = Vector2.right;
		_magnitude = Vector2.one;
	}

	private void KeyDown()
	{
		_keyDown = true;
	}
	public void KeyUp()
	{
		_keyDown = false;
		ClearData();
	}

	public void Interaction()
	{
		if (_selectedClickable != null)
		{
			ArtworkController.Instance.ArtworkSelected(_selectedClickable);
		}
	}
}
