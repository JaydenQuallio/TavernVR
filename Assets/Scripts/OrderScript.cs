using UnityEngine;

public class OrderScript : MonoBehaviour, IOrderInterface
{

    [SerializeField]
    private Transform stickPoint;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Collider noteCollider;

    [SerializeField]
    private LayerMask avoidLayer;

    private int orderNum = 0;

    private IPatronInterface patron; 

    private bool isPickedUp = false;

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
            Debug.Log(hit.collider.name);
            if (transform.parent != hit.transform)
            {
                transform.SetParent(hit.transform);
                rb.isKinematic = true;
                noteCollider.enabled = false;
            }
        }
        else
        {
            transform.SetParent(null);
            rb.isKinematic = false;
            noteCollider.enabled = true;

        }

        Debug.DrawRay(stickPoint.position, stickPoint.TransformDirection(Vector3.down), Color.red, .02f);
    }

    public void OnPickUp() => isPickedUp = true;
    public void OnDropped() => isPickedUp = false;

}
