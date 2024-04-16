using UnityEngine;

public class LeaveTrigger : MonoBehaviour
{
	[SerializeField] GameObject leaveArea;
	
	private void OnTriggerEnter(Collider coll)
	{
		leaveArea.SetActive(true);	
	}
	
	private void OnTriggerExit(Collider coll)
	{
		leaveArea.SetActive(false);	
	}
}
