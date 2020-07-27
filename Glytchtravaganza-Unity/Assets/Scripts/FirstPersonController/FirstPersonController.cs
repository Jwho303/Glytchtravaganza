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

	private void Start()
	{
		InputController.Instance.SubscribeToTranslate(Translate);
		InputController.Instance.SubscribeToRotation(Rotate);
		InputController.Instance.SubscribeToTap(Tap);
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
