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

            if (tempOrder.HasDrink())
            {
                GradeDrink(tempOrder.CompareDrink());
            }
        }
    }

    private void GradeDrink(float percent)
    {
        if(percent >= .99f)
        {

        }
        else if(percent >= .9f)
        {

        }
        else if(percent >= .8f)
        {

        }
        else if(percent >= .7)
        {

        }
        else if (percent >= .6)
        {

        }
        else if (percent >= .5)
        {

        }
        else
        {

        }
    }
}
