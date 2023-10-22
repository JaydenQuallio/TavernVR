using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class DrinksManager : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<string, OrderScriptable> orders = new();

    public static DrinksManager Instance = new();

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

    public OrderScriptable GetOrder(string orderName)
    {
        if (orders[orderName])
        {
            Debug.Log(orderName);
            return orders[orderName];
        }
        else
        {
            Debug.Log("Could not find order");
            return null;
        }
    }
}
