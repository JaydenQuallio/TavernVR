using System.Collections;
using System.Collections.Generic;
using Autohand;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] List<OrderScriptable> orders = new();
	
	[SerializeField] private OrderScript tempOrder;
	[SerializeField] private Grabbable grabScript;
	[SerializeField] Transform noteTrans;
	[SerializeField] Animator frogAnim;
	[SerializeField] GameObject tryAgainText;
	
	int count = 0;
	
	private void Start()
	{
		SetOrder();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Note"))
		{
			if (tempOrder.HasDrink())
			{
				GameObject temp = tempOrder.transform.parent.gameObject;

				tempOrder.GetDrinks(temp);

				GradeDrink(tempOrder.CompareDrink());
				grabScript.enabled = false;
				tempOrder.transform.parent = null;

				tempOrder.ClearOrder();
				SetOrder();
				tempOrder.transform.position = noteTrans.position;
				grabScript.enabled = true;
				
				temp.SetActive(false);
			}
		}
	}

	private void SetOrder()
	{
		tempOrder.SetOrder(count, orders[Random.Range(0, orders.Count - 1)]);
		count++;
	}

	private void GradeDrink(float percent)
	{
		Debug.Log(percent);

		if (percent >= .5)
		{
			Debug.Log("Good");
			frogAnim.SetTrigger("NodHead");
			SceneChangeManager.instance.CallSceneChange("Intro");
		}
		else
		{
			tryAgainText.SetActive(true);
			frogAnim.SetTrigger("ShakeHead");
			Invoke(nameof(Fail), 2f);
			Debug.Log("Bad");
		}
	}
	
	private void Fail()
	{
		tryAgainText.SetActive(false);
	}
}
