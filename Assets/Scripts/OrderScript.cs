using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class OrderScript : SerializedMonoBehaviour, IOrderInterface
{
    [SerializeField]
    private Transform stickPoint;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private LayerMask avoidLayer;

    private int orderNum = 0;

    private IPatronInterface patron;

    private bool isPickedUp = false;

    [SerializeField]
    private OrderScriptable order;

    [ShowInInspector]
    private List<IGrogInterface> drinks = new List<IGrogInterface>();

    private void OnEnable()
    {
        PatronManager.Instance.InitialAddToOrderList(this);
    }

    public void SetOrder(int orderNumber, IPatronInterface patronInterface)
    {
        orderNum = orderNumber;
        patron = patronInterface;
        Debug.Log("Ordered");
    }

    private void Update()
    {
        if (isPickedUp)
            return;

        RaycastHit hit;
        if (Physics.Raycast(stickPoint.transform.position, stickPoint.TransformDirection(Vector3.down), out hit, .02f, ~avoidLayer))
        {
            if (transform.parent != hit.transform)
            {
                transform.SetParent(hit.transform);
                rb.isKinematic = true;
                GetDrinks(hit.transform.gameObject);
            }
        }
        else
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            drinks.Clear();
        }

        Debug.DrawRay(stickPoint.position, stickPoint.TransformDirection(Vector3.down), Color.red, .02f);
    }

    public void OnPickUp() => isPickedUp = true;
    public void OnDropped() => isPickedUp = false;

    public void GetDrinks(GameObject parents)
    {
        if (drinks.Contains(parents.transform.GetComponent<IGrogInterface>()))
            return;

        Debug.Log("adding drink");

        if (parents.CompareTag("Drink"))
                drinks.Add(parents.transform.GetComponent<IGrogInterface>());
        else
        {
            for (int i = 0; i < parents.transform.childCount; i++)
            {
                Debug.Log(parents.transform.GetChild(i));

                if (parents.transform.GetChild(i).CompareTag("Drink"))
                {
                    drinks.Add(parents.transform.GetChild(i).GetComponent<IGrogInterface>());
                }
            }
        }
        Debug.Log(drinks.Count);

    }

    public void ClearDrinks() => drinks.Clear();

}
