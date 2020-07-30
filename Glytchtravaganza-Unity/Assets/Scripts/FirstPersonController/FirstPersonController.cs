using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
	[SerializeField]
	private float _translationSpeed = 5f;
	[SerializeField]
	private float _rotationSpeed = 5f;
	[SerializeField]
	private Camera _viewCamera;

	[SerializeField]
	private Rigidbody rigidbody;

	private bool _facingArtwork = true;

	private void Start()
	{
		InputController.Instance.SubscribeToTranslate(Translate);
		InputController.Instance.SubscribeToRotation(Rotate);
		InputController.Instance.SubscribeToTap(Tap);
	}

	private void Update()
	{
		Ray cameraRay = _viewCamera.ScreenPointToRay(new Vector2(Screen.width/2f,Screen.height/2f));

		RaycastHit rayCastHit;
		if (Physics.Raycast(cameraRay, out rayCastHit))
		{
			ArtworkClickable artwork = rayCastHit.collider.gameObject.GetComponent<ArtworkClickable>();
			if (artwork != null && !_facingArtwork)
			{
				HitArtwork(artwork);
			}
			else if (artwork == null && _facingArtwork)
			{
				MissedArtwork();
			}
		}
	}

	private void MissedArtwork()
	{
		InputController.Instance.MissedArtwork();
		_facingArtwork = false;
	}

	private void HitArtwork(ArtworkClickable artwork)
	{
		InputController.Instance.HitArtwork(artwork);
		_facingArtwork = true;
	}

	private void Translate(float direction, float magnitude)
	{
		rigidbody.velocity = this.transform.forward * _translationSpeed * direction * magnitude * Time.fixedDeltaTime;
	}

	private void Rotate(float direction, float magnitude)
	{
		rigidbody.angularVelocity = new Vector3(0f, _rotationSpeed * direction * magnitude * Time.fixedDeltaTime, 0f);
	}

	private void Tap(Vector2 position)
	{
		Ray cameraRay = _viewCamera.ScreenPointToRay(position);

		RaycastHit rayCastHit;
		if (Physics.Raycast(cameraRay, out rayCastHit))
		{
			//Debug.Log("Hit :" + rayCastHit.collider.gameObject.name);
			InputController.Instance.Hit(rayCastHit.collider.gameObject);
		}
	}
}
