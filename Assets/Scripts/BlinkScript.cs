using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    [SerializeField]
    private GameObject eyeLidsOpen, eyeLidsClose;

    [SerializeField]
    private float blinkTime, blinkHold;
    private bool isBlinked = false;

    private void Start()
    {
        SetBlinkTimes();
    }

    private void FixedUpdate()
    { 
        if(blinkTime > 0f)
        {
            if (!isBlinked)
            {
                isBlinked = true;
                eyeLidsClose.SetActive(false);
                eyeLidsOpen.SetActive(true);
            }

            blinkTime -= Time.fixedDeltaTime;
        }
        else if(blinkHold > 0f)
        {
            if (isBlinked)
            {
                isBlinked = false;
                eyeLidsOpen.SetActive(false);
                eyeLidsClose.SetActive(true);
            }

            blinkHold -= Time.fixedDeltaTime;
        }
        else
        {
            SetBlinkTimes();
        }
    }

    private void SetBlinkTimes()
    {
        blinkTime = Random.Range(.5f, 5f);
        blinkHold = Random.Range(.25f, 1.75f);
    }

}
