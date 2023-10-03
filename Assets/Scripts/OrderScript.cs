using UnityEngine;

public class OrderScript : MonoBehaviour
{

    [SerializeField]
    private Transform stickPoint;

    [SerializeField]
    private Rigidbody rb;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(stickPoint.transform.position, stickPoint.TransformDirection(Vector3.down), out hit, .02f))
        {
            if (transform.parent != hit.transform)
            {
                transform.SetParent(hit.transform);
                rb.isKinematic = true;
            }
        }
        else
        {
            transform.SetParent(null);
            rb.isKinematic = false;
        }

        Debug.DrawRay(stickPoint.position, stickPoint.TransformDirection(Vector3.down), Color.red, .02f);
    }

}
