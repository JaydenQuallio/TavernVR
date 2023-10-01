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
    private IGrogInterface currentGrog;

    private void Start()
    {
        GrogManager.Instance.AddBarrelToList(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isPouring || !other.CompareTag("Grog"))
        {
            return;
        }

        Debug.Log(other.gameObject);

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

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Grog"))
        {
            return;
        }

        Debug.Log(other.gameObject);

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
