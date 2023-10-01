using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrogManager : MonoBehaviour
{
    public static GrogManager Instance { get; private set; }

    [SerializeField]
    private List<IGrogInterface> Grogs = new();

    [SerializeField]
    private List<GrogBarrel> Barrels = new();

    private Dictionary<GameObject, IGrogInterface> grogsDictionary = new();

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

    public void AddBarrelToList(GrogBarrel grogBarrel)
    {
        if (Barrels.Contains(grogBarrel))
            return;

        Barrels.Add(grogBarrel);
        grogBarrel.SetGrogDictionary(grogsDictionary);
    }

    
}
