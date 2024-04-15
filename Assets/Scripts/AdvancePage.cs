using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class AdvancePage : MonoBehaviour
{
	[SerializeField] BookScript bs;
	[SerializeField] HandPublicEvents handEvs;
	[SerializeField] bool isNextPage = false;

	private void NextPage(Hand hand, Grabbable grab)
	{
		if (isNextPage)
			bs.ToggleFwdPage();
		else
			bs.ToggleBwdPage();
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.TryGetComponent<HandPublicEvents>(out HandPublicEvents handEvents))
		{
			Debug.Log("Call");
			handEvents.OnSqueeze.AddListener(NextPage);
		}
	}

	private void OnTriggerExit(Collider coll)
	{
		if (coll.TryGetComponent<HandPublicEvents>(out HandPublicEvents handEvents))
		{
			Debug.Log("Call");
			handEvents.OnSqueeze.RemoveListener(NextPage);
		}
	}
}
