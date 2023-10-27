using UnityEngine;
using TMPro;
using System.Text;

public class FinishOrder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cashAmount;

    private StringBuilder sb = new();

    private OrderScript tempOrder;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            tempOrder = other.GetComponent<OrderScript>();

            if (tempOrder.HasDrink())
            {
                GameObject temp = tempOrder.transform.parent.gameObject;

                tempOrder.GetDrinks(temp);

                GradeDrink(tempOrder.CompareDrink());

                //Sets the text for how much cash there is
                sb.Clear();
                sb.Append("Cash: $").Append(gameManager.GetCoin().ToString());
                cashAmount.text = sb.ToString();

                //Change this to consuming wait at later time for sitting at a table
                tempOrder.ChangePlayerState(PlayerStates.Finished);


                tempOrder.transform.parent = null;

                tempOrder.ClearOrder();

                temp.SetActive(false);
                tempOrder.gameObject.SetActive(false);
            }
        }
    }

    private void GradeDrink(float percent)
    {
        if(percent >= .99f)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * 2f);
        }
        else if(percent >= .9f)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * 1.75f);
        }
        else if(percent >= .8f)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * 1.5f);
        }
        else if(percent >= .7)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * 1.25f);
        }
        else if (percent >= .6)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * 1f);
        }
        else if (percent >= .5)
        {
            gameManager.SetCoin(gameManager.GetBaseCoin() * .75f);
        }
        else
        {
            gameManager.SetCoin(-gameManager.GetBaseCoin());
        }
    }
}
