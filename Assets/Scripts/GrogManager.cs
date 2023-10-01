using System.Collections.Generic;
using UnityEngine;

public class GrogManager : MonoBehaviour
{
    public static GrogManager Instance { get; private set; }

    [SerializeField]
    private List<IGrogInterface> Grogs = new();

    [SerializeField]
    private bool IsPouring = false;

    private IGrogInterface currentGrog;
    private GameObject currentGrogObject;

    private Dictionary<GameObject, IGrogInterface> grogsDictionary = new();

    [SerializeField]
    private DrinkTypes drinkType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGrogToList(IGrogInterface grog, GameObject grogObject)
    {
        if (Grogs.Contains(grog))
            return;

        Grogs.Add(grog);

        grogsDictionary.Add(grogObject, grog);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsPouring || !other.CompareTag("Grog"))
        {
            return;
        }

        Debug.Log(other.gameObject);

        if (currentGrogObject != null)
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
}
