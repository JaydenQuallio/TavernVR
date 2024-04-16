using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerScript : MonoBehaviour
{
	[SerializeField] Animator fadeOutAnimator;
	[SerializeField] SoundScript walkSounds;

	[SerializeField] Rigidbody rb;

	float stepTime = 0f, nextStepTime = .8f;

	void Start()
	{
		SceneChangeManager.instance.SetAnim(fadeOutAnimator);
	}

	private void FixedUpdate()
	{
		if(rb == null)
			return;
		
		if (rb.velocity.x != 0f || rb.velocity.z != 0f)
		{
			if (stepTime < nextStepTime)
				stepTime += Time.fixedDeltaTime;
			else
			{
				stepTime = 0f;
				walkSounds.PlayAudioSource();
			}
		}
	}
}
