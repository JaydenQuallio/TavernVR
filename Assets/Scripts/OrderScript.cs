using UnityEngine;
using System.Collections.Generic;

public class OrderScript : MonoBehaviour, IOrderInterface
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

    private void GetDrinks(GameObject parent)
    {
        foreach(GameObject obj in parent.GetComponents<GameObject>())
        {
            if (obj.CompareTag("Drink"))
            {
                drinks.Add(obj.GetComponent<IGrogInterface>());
            }
        }
    }

}
