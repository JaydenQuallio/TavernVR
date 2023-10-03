using System.Collections.Generic;
using UnityEngine;

public class GrogBarrel : MonoBehaviour
{
    private Dictionary<GameObject, IGrogInterface> grogsDictionary = new();

    [SerializeField]
    private DrinkTypes drinkType;

    [SerializeField]
    private bool isPouring = false;
    
    private GameObject currentGrogObject;

    [SerializeField]
    private GameObject spout, ParticleParent;

    private IGrogInterface currentGrog;

    [SerializeField]
    private LineRenderer pourRenderer;

    [SerializeField]
    ParticleSystem splashPart, carbonationPart;

    private void Start()
    {
        GrogManager.Instance.AddBarrelToList(this);
    }

    [SerializeField]
    private float amplitude = 0.003f, waveFreq = 4.5f, movementSpeed = 10.35f;

    [SerializeField]
    private int points = 50;

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPouring || !other.CompareTag("Drink"))
        {
            return;
        }

        pourRenderer.enabled = true;
        splashPart.Play();
        carbonationPart.Play();

    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPouring || !other.CompareTag("Drink"))
        {
            return;
        }

        DrawLineSine(other.transform.position);

        if (currentGrogObject == other.gameObject)
        {
            Debug.Log("Filling");
            currentGrog.FillGrog(drinkType, .1f * Time.fixedDeltaTime);
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

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Drink"))
        {
            return;
        }
        
        pourRenderer.enabled = false;
        splashPart.Stop();
        carbonationPart.Stop();

        if (currentGrogObject == other.gameObject)
        {
            Debug.Log("Done Pouring");
            currentGrog.StopGrog();
        }
    }

    public void SetGrogDictionary(Dictionary<GameObject, IGrogInterface> dictionary)
    {
        grogsDictionary = dictionary;
    }
}
