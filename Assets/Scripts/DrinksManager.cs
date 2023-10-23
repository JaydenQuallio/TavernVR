using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class DrinksManager : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<int, OrderScriptable> orders = new();

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

    public OrderScriptable GetOrder(int orderNumber)
    {
        if (orders[orderNumber])
        {
            Debug.Log(orderNumber);
            return orders[orderNumber];
        }
        else
        {
            Debug.Log("Could not find order");
            return null;
        }
    }

    public int DrinkCount => orders.Count;
}
