using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHider : GlitchListener
{
	[SerializeField]
	private GameObject _childGameObject;

	public override void Start()
	{
		_onAction += ShowChild;
		_offAction += HideChild;
		base.Start();
	}

	private void ShowChild()
	{
		_childGameObject.SetActive(true);
	}

	private void HideChild()
	{
		_childGameObject.SetActive(false);
	}

}
