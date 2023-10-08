using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PatronManager : SerializedMonoBehaviour
{
    [SerializeField]
    public Dictionary<int, Transform> LinePoints = new();

    public List<int> openPoints = new();
    public List<int> takenPoints = new();
    public List<Transform> roamingPoints = new();

    [SerializeField]
    private List<IPatronInterface> Patrons = new();
    private List<IOrderInterface> Orders = new();


    [SerializeField]
    private BoxCollider standArea;

    int patronCount = 0;

    private float chanceToEnter = 1.33f;

    public static PatronManager Instance { get; private set; }

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

        foreach(KeyValuePair<int, Transform> kvp in LinePoints)
        {
            openPoints.Add(kvp.Key);
        }
    }

    public void InitialAddToOrderList(IOrderInterface order)
    {
        if (Orders.Contains(order))
            return;

        Orders.Add(order);
    }

    public void InitialAddToPatronList(IPatronInterface patron)
    {
        if (Patrons.Contains(patron))
            return;

        Patrons.Add(patron);

        patronCount++;
        Patrons[patronCount - 1].SetPatronNumber(patronCount);
        Patrons[patronCount - 1].SetLineDictionary(LinePoints);
    }

    public void UpdateOrderInfo(IPatronInterface patron, IOrderInterface order, int orderNumber)
    {
        patron.SetPatronNumber(orderNumber);
        order.SetOrder(orderNumber, patron);
    }

    // Method used to progress the line when needed
    [Button]
    public void ProgressLine()
    {
        foreach (IPatronInterface patron in Patrons)
        {
            patron.AdvanceLine();
        }
    }

    // Removes the point from the openPoints list to not let any other point move there
    public void CloseSpot(int pointPos)
    {
        openPoints.Remove(pointPos);
        //takenPoints.Add(pointPos);
    }

    // Takes int and adds it back to the openPoints list and sorts them to make sure they stay in order
    public void OpenSpot(int pointPos)
    {
        //takenPoints.Remove(pointPos);
        openPoints.Add(pointPos);
        openPoints.Sort();
    }

    // Generates a random spot within the bounds provided
    public Vector3 GenerateSpot() => new(standArea.transform.position.x - Random.Range(-standArea.bounds.extents.x, standArea.bounds.extents.x), 0, standArea.transform.position.z - Random.Range(-standArea.bounds.extents.z, standArea.bounds.extents.z));

    public float ChanceModifier { get { return chanceToEnter; } set { chanceToEnter = value; } }
}
