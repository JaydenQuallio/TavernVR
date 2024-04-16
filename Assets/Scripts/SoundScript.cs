using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
	[SerializeField] AudioSource audioSource;

	[SerializeField] bool isSemiRandom;

	[ShowInInspector] float startR = .75f, endR = 1.2f;

	[SerializeField] int audioIndex = 0;

	float defaultPitch = 1;

	[SerializeField] List<AudioClip> audioClips = new();

	public void SetAudioIndex(int index) => audioIndex = index;

	public void PlayAudioSource()
	{
		audioSource.clip = audioClips[audioIndex];
		
		if (isSemiRandom)
		{
			audioSource.pitch = Random.Range(startR, endR);
		}
		Debug.Log("Playing " + audioClips[audioIndex]);
		audioSource.Play();
	}
}
