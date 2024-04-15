using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class GrogBarrel : MonoBehaviour
{
	private Dictionary<GameObject, IGrogInterface> grogsDictionary = new();

	[TabGroup("Drink Settings")]
	[SerializeField] DrinkTypes drinkType, drinkLook;

	[TabGroup("Particle Systems")]
	[SerializeField] ParticleSystem splashPart, carbonationPart, foamPart;

	[BoxGroup("References")]
	[SerializeField] GameObject spout, ParticleParent;

	[BoxGroup("References")]
	[SerializeField]
	private LineRenderer pourRenderer;

	[BoxGroup("References")]
	[SerializeField] HingeJoint lever;

	private GameObject currentGrogObject;

	private References references;

	private IGrogInterface currentGrog;

	private bool isPlaying;

	[BoxGroup("Parameters")]
	[SerializeField] float amplitude = 0.003f, waveFreq = 4.5f, movementSpeed = 10.35f;

	[BoxGroup("Parameters")]
	[SerializeField] int points = 50;

	private AnimationCurve curve = new();

	private bool pourLiquid = false;

	private void Awake()
	{
		references = References.instance;
	}

	private void Start()
	{
		GrogManager.Instance.AddBarrelToList(this);
		splashPart.GetComponent<Renderer>().material = references.GetDrinkMaterial(drinkLook);
		pourRenderer.GetComponent<Renderer>().material = references.GetDrinkMaterial(drinkLook);
	}


	private void Update()
	{
		if (pourLiquid)
		{
			curve = AnimationCurve.EaseInOut(0f, .33f, 1f, .2f);
			pourRenderer.widthCurve = curve;

			DetectDrink();
		}
		else if (lever.angle <= 2f)
		{
			pourRenderer.enabled = false;

			if (isPlaying)
			{
				StartStopAnimations(false);
			}
		}
		else
		{
			curve = AnimationCurve.EaseInOut(0f, Mathf.Lerp(0f, .33f, lever.angle / lever.limits.max), 1f, Mathf.Lerp(0f, .2f, lever.angle / lever.limits.max));
			pourRenderer.widthCurve = curve;

			DetectDrink();
		}
	}

	private void DetectDrink()
	{
		RaycastHit hit;

		if (Physics.Raycast(spout.transform.position, Vector3.down, out hit, 1000f))
		{
			DrawLineSine(hit.point);

			foamPart.transform.position = hit.point;

			pourRenderer.enabled = true;

			if (!isPlaying)
			{
				StartStopAnimations(true);
			}

			FillDrink(hit.collider);
		}
	}
	private void FillDrink(Collider other)
	{
		if (currentGrogObject == other.gameObject)
		{
			float tempAmount = Mathf.Lerp(.05f, .2f, lever.angle / lever.limits.max);
			currentGrog.FillGrog(drinkType, tempAmount * Time.fixedDeltaTime);
		}
		else
		{
			if (grogsDictionary.ContainsKey(other.gameObject))
			{
				currentGrog = grogsDictionary[other.gameObject];
				currentGrogObject = other.gameObject;
			}
		}
	}

	private void DrawLineSine(Vector3 cupTrans)
	{
		float xStart = 0;
		float Tau = 2 * Mathf.PI;
		float xFinish = cupTrans.y - spout.transform.position.y;

		pourRenderer.positionCount = points;

		ParticleParent.transform.localScale = new Vector3(1, -xFinish, 1);

		for (int currentPoint = 0; currentPoint < points; currentPoint++)
		{
			float progress = (float)currentPoint / (points - 1);
			float x = Mathf.Lerp(xStart, xFinish, progress);
			float y = amplitude * Mathf.Sin((Tau * waveFreq * x) + (Time.timeSinceLevelLoad * movementSpeed));
			pourRenderer.SetPosition(currentPoint, new Vector3(y, x, 0));
		}
	}

	[Button("TestLiquid"), HideInInspector]
	private void TestLiquidEditor()
	{
		pourLiquid = !pourLiquid;
		Debug.Log(pourLiquid);
	}

	public void SetGrogDictionary(Dictionary<GameObject, IGrogInterface> dictionary)
	{
		grogsDictionary = dictionary;
	}

	/// <summary>
	/// Pass in true or false to either make the animations play true or stop false
	/// </summary>
	/// <param name="play">Start or stop animations for spout</param>
	private void StartStopAnimations(bool play)
	{
		if (play)
		{
			splashPart.Play();
			carbonationPart.Play();
			foamPart.Play();
			isPlaying = play;
		}
		else
		{
			splashPart.Stop();
			carbonationPart.Stop();
			foamPart.Stop();
			isPlaying = play;
		}
	}
}
