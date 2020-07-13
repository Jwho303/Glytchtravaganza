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

	public void LateUpdate()
	{
		if (_isDragging || _keyDown)
		{
			InputController.Instance.Translate(_direction.y, _magnitude.y);
			InputController.Instance.Rotate(_direction.x, _magnitude.x);
		}
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.LogFormat("[{0}] Begin Dragging", this.name);
		_isDragging = true;
		_dragOrigin = eventData.position;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.LogFormat("[{0}] On Dragging", this.name);
		_direction = (eventData.position - _dragOrigin).normalized;
		_magnitude.x = Mathf.Clamp(Mathf.Abs(eventData.position.x - _dragOrigin.x), 0f, _maxMagnitude) / _maxMagnitude;
		_magnitude.y = Mathf.Clamp(Mathf.Abs(eventData.position.y - _dragOrigin.y), 0f, _maxMagnitude) / _maxMagnitude;

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.LogFormat("[{0}] End Dragging", this.name);
		_isDragging = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_isDragging)
		{
			Debug.LogFormat("[{0}] Pointer Click", this.name);
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
		ClearData();
	}
	public void KeyUp()
	{
		_keyDown = false;
		ClearData();
	}
}
