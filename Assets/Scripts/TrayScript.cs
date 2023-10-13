using UnityEngine;

public class TrayScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            other.transform.GetComponent<Rigidbody>().mass = 0;
            other.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.transform.GetComponent<Rigidbody>().mass = 1;
            other.transform.SetParent(null, true);
        }
    }

}
