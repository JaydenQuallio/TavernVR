using UnityEngine;
using System.Text;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class OrderScript : MonoBehaviour, IOrderInterface
{
    [TabGroup("Components")]
    [SerializeField]
    private Transform stickPoint;

    [TabGroup("Components")]
    [SerializeField]
    private Rigidbody rb;

    [TabGroup("Components")]
    [SerializeField]
    private TextMeshPro noteText;

    [TabGroup("NoteSettings")]
    [SerializeField]
    private OrderScriptable order;

    [TabGroup("NoteSettings")]
    [SerializeField]
    private LayerMask avoidLayer;

    [BoxGroup("Parent")]
    [SerializeField]
    private PatronAI patron;

    [ShowInInspector]
    private List<float> drinkValues = new();

    private int orderNum = 0;

    private IGrogInterface drinks;

    private StringBuilder sb = new();

    private bool isPickedUp = false, hasBeenTouched = false;

    public OrderScriptable GenerateOrder()
    {
        return DrinksManager.Instance.GetOrder(Random.Range(0, DrinksManager.Instance.DrinkCount));
    }

    public void SetOrder(int orderNumber, OrderScriptable orderScriptable)
    {
        orderNum = orderNumber;
        order = orderScriptable;

        sb.Append("Patron #").Append(orderNum).AppendLine("\n1 x ").Append(order.drinkName);

        noteText.text = sb.ToString();

        Debug.Log("Ordered");
    }

    public void ClearOrder()
    {
        sb.Clear();
        ClearDrinks();
        hasBeenTouched = false;
        isPickedUp = false;
        drinkValues.Clear();
    }

    private void Update()
    {
        if (isPickedUp || !hasBeenTouched)
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
        else if(hasBeenTouched)
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            drinks = null;
        }

        Debug.DrawRay(stickPoint.position, stickPoint.TransformDirection(Vector3.down), Color.red, .02f);
    }

    [Button("Simulate Grab")]
    private void TestGrab()
    {
        OnPickUp();
        OnDropped();
    }

    public void OnPickUp()
    {
        isPickedUp = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (!hasBeenTouched)
        {
            PatronManager.Instance.ProgressLine();
            hasBeenTouched = true;
        }
    }

    public void OnDropped()
    {
        rb.useGravity = true;
        isPickedUp = false;
    }

    public void GetDrinks(GameObject parents)
    {
        IGrogInterface tempInterface = parents.transform.GetComponent<IGrogInterface>();

        if (drinks == tempInterface)
            return;

        Debug.Log("adding drink");

        if (parents.CompareTag("Drink"))
        {
            drinks = tempInterface;

            drinkValues = tempInterface.GetAllDrinks();
        }
    }

    public void ClearDrinks()
    {
        drinks = null;
    }

    public void ChangePlayerState(PlayerStates state)
    {
        patron.SetOrderState(state);
    }

    public bool HasDrink() => drinks != null;

    public float CompareDrink()
    {
        order.CalculateDrink(
            drinks.GetDrink(DrinkTypes.RedPotion), 
            drinks.GetDrink(DrinkTypes.GreenPotion), 
            drinks.GetDrink(DrinkTypes.BluePotion), 
            drinks.GetDrink(DrinkTypes.WhitePotion),
            drinks.GetDrink(DrinkTypes.BlackPotion)
            );

        return order.CompareDrink(drinks.GetFillAmount(), drinks.GetMaxAmount());
    }
}
