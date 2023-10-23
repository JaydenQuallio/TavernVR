using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Text;
using TMPro;

public class OrderScript : SerializedMonoBehaviour, IOrderInterface
{
    [SerializeField]
    private Transform stickPoint;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private LayerMask avoidLayer;

    [SerializeField]
    private Transform notesParent;

    private int orderNum = 0;

    private IPatronInterface patron;

    private bool isPickedUp = false;

    [SerializeField]
    private OrderScriptable order;

    [ShowInInspector]
    private IGrogInterface drinks;

    private StringBuilder sb = new();

    [SerializeField]
    private TextMeshProUGUI tmp;

    [ShowInInspector]
    private Vector3 drinkValues = new();

    private void OnEnable()
    {
        PatronManager.Instance.InitialAddToOrderList(this);
    }

    private void Start()
    {
        SetOrder(PatronManager.Instance.GetPatronOrder(), GetOrder());
    }

    private OrderScriptable GetOrder()
    {
        return DrinksManager.Instance.GetOrder(Random.Range(0, DrinksManager.Instance.DrinkCount - 1));
    }

    public void SetOrder(int orderNumber, OrderScriptable orderScriptable)
    {
        orderNum = orderNumber;
        order = orderScriptable;

        sb.Append("Patron #").Append(orderNum).AppendLine("\n1 x ").Append(order.drinkName);

        tmp.text = sb.ToString();

        Debug.Log("Ordered");
    }

    private void Update()
    {
        if (isPickedUp )
            return;

        RaycastHit hit;
        if (Physics.Raycast(stickPoint.transform.position, stickPoint.TransformDirection(Vector3.down), out hit, .035f, ~avoidLayer))
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
            transform.SetParent(notesParent);
            rb.isKinematic = false;
            drinks = null;
        }

        Debug.DrawRay(stickPoint.position, stickPoint.TransformDirection(Vector3.down), Color.red, .02f);
    }

    public void OnPickUp()
    {
        isPickedUp = true;
        Debug.Log("Is not kinematic");
        rb.isKinematic = false;
    }
    public void OnDropped() => isPickedUp = false;

    public void GetDrinks(GameObject parents)
    {
        IGrogInterface tempInterface = parents.transform.GetComponent<IGrogInterface>();

        if (drinks == tempInterface)
            return;

        Debug.Log("adding drink");

        if (parents.CompareTag("Drink"))
        {
            drinks = tempInterface;
            drinkValues = tempInterface.GetDrink();
        }
    }

    public void ClearDrinks()
    {
        drinks = null;
    }    
}
