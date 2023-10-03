using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class References : SerializedMonoBehaviour
{
    public static References instance;

    [SerializeField]
    private Dictionary<DrinkTypes, Material> drinkTypeList = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Material GetDrinkMaterial(DrinkTypes drinkType)
    {
        if (drinkTypeList.TryGetValue(drinkType, out Material drinkMaterial))
        {
            return drinkMaterial;
        }

        Debug.LogWarning($"Could not find Drink Type: {drinkType} list");
        return null;
    }

    public Dictionary<DrinkTypes, Material> DrinkTypeList
    {
        get { return drinkTypeList; }
    }
}
