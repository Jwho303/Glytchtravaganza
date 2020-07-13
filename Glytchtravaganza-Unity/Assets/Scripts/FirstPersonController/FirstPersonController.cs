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
	private Rigidbody rigidbody;

	private void Start()
	{
		InputController.Instance.SubscribeToTranslate(Translate);
		InputController.Instance.SubscribeToRotation(Rotate);
	}

	private void Translate(float direction, float magnitude)
	{
		rigidbody.AddRelativeForce(new Vector3(0f, 0f, _translationSpeed * direction * magnitude), ForceMode.Acceleration);
	}

	private void Rotate(float direction, float magnitude)
	{
		rigidbody.AddRelativeTorque(new Vector3(0f, _rotationSpeed * direction * magnitude, 0f), ForceMode.Acceleration);
	}
}
