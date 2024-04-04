using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeTriggerAnim : MonoBehaviour
{
	[SerializeField] Animation animations;
	[SerializeField] AnimationClip clip1, clip2;

	private void OnTriggerEnter(Collider coll)
	{

		animations.clip = null;
		animations.clip = clip1;
		animations.Play();
	}

	private void OnTriggerExit(Collider coll)
	{
		animations.clip = null;
		animations.clip = clip2;
		animations.Play();
	}
}
