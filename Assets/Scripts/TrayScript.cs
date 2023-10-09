using UnityEngine;

public class TrayScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.SetParent(null, true);
        }
    }

}
