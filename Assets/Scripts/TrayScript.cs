using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class TrayScript : SerializedMonoBehaviour
{
    [ShowInInspector]
    private List<IOrderInterface> objects = new List<IOrderInterface>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            other.transform.GetComponent<Rigidbody>().mass = 0;
            other.transform.SetParent(transform, true);

            foreach (IOrderInterface grogs in objects)
            {
                grogs.GetDrinks(gameObject);
            }
        }
        else if (other.CompareTag("Note"))
        {
            IOrderInterface order = other.GetComponent<IOrderInterface>();
            objects.Add(order);
            order.GetDrinks(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drink"))
        {
            other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.transform.GetComponent<Rigidbody>().mass = 1;
            other.transform.SetParent(null, true);

            foreach (IOrderInterface grogs in objects)
            {
                Debug.Log(grogs + "Clearnign");
                grogs.ClearDrinks();
            }
        }
        else if (other.CompareTag("Note"))
        {
            IOrderInterface order = other.GetComponent<IOrderInterface>();
            order.ClearDrinks();
            objects.Remove(order);
        }
    }

}
