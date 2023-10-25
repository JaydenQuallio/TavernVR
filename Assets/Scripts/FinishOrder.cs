using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishOrder : MonoBehaviour
{
    private OrderScript tempOrder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            tempOrder = other.GetComponent<OrderScript>();
        }
    }

    private void GradeDrink(float percent)
    {
        if(percent > .95f)
        {

        }
        else if(percent > .9f)
        {

        }
        else if(percent > .75f)
        {

        }
    }
}
