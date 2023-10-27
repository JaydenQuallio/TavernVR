using System.Collections.Generic;
using UnityEngine;

public class GrogBarrel : MonoBehaviour
{
    private Dictionary<GameObject, IGrogInterface> grogsDictionary = new();

    [SerializeField]
    private DrinkTypes drinkType, drinkLook;

    private GameObject currentGrogObject;

    [SerializeField]
    private GameObject spout, ParticleParent;

    private IGrogInterface currentGrog;

    [SerializeField]
    private LineRenderer pourRenderer;

    [SerializeField]
    private ParticleSystem splashPart, carbonationPart, foamPart;

    private References references;

    [SerializeField]
    private HingeJoint lever;

    private bool isPlaying;

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

    [SerializeField]
    private float amplitude = 0.003f, waveFreq = 4.5f, movementSpeed = 10.35f;

    [SerializeField]
    private int points = 50;

    private AnimationCurve curve = new();

    private void Update()
    {
        if (lever.angle >= 0f)
        {
            pourRenderer.enabled = false;

            if (isPlaying)
            {
                splashPart.Stop();
                carbonationPart.Stop();
                foamPart.Stop();
                isPlaying = false;
            }
        }
        else
        {
            curve = AnimationCurve.EaseInOut(0f, Mathf.Lerp(0f, .33f, lever.angle / lever.limits.min), 1f, Mathf.Lerp(0f, .2f, lever.angle / lever.limits.min));
            pourRenderer.widthCurve = curve;
            RaycastHit hit;

            if (Physics.Raycast(spout.transform.position, transform.TransformDirection(Vector3.down), out hit, 1000f))
            {
                DrawLineSine(hit.point);

                foamPart.transform.position = hit.point;

                pourRenderer.enabled = true;

                if (!isPlaying)
                {
                    splashPart.Play();
                    carbonationPart.Play();
                    foamPart.Play();
                    isPlaying = true;
                }

                FillDrink(hit.collider);
            }
        }
    }

    private void FillDrink(Collider other)
    {
        if (currentGrogObject == other.gameObject)
        {
            float tempAmount = Mathf.Lerp(.05f, .1f, lever.angle / lever.limits.min);

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

    public void SetGrogDictionary(Dictionary<GameObject, IGrogInterface> dictionary)
    {
        grogsDictionary = dictionary;
    }
}
