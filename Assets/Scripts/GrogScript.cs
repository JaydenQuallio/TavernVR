using System.Collections.Generic;
using UnityEngine;

public class GrogScript : MonoBehaviour, IGrogInterface
{
    private Dictionary<DrinkTypes, float> drinkList = new();

    [SerializeField]
    private LiquidScript liquidScript;

    [SerializeField]
    private float totalLiquid;

    [SerializeField]
    private Material liquidMat, tempMat;

    private void Start()
    {
        liquidScript.enabled = false;
    }

    private void OnEnable()
    {
        tempMat =  new Material(liquidMat);
        GrogManager.Instance.AddGrogToList(this, gameObject);
        ResetPotionStats();
    }

    private void OnDisable()
    {
        Destroy(tempMat);
        liquidScript.enabled = false;
    }

    public void FillGrog(DrinkTypes drinkType, float fillAmount)
    {
        if(liquidScript.enabled == false)
            liquidScript.enabled = true;

        totalLiquid += fillAmount;
        drinkList[drinkType] += fillAmount;
        IncreaseLiquid(totalLiquid);
    }

    private void IncreaseLiquid(float liquidAmount)
    {
        liquidScript.SetAmount = Mathf.Lerp(.556f, .45f, liquidAmount);
    }

    void ResetPotionStats()
    {
        drinkList.Add(DrinkTypes.RedPotion, 0);
        drinkList.Add(DrinkTypes.BluePotion, 0);
        drinkList.Add(DrinkTypes.GreenPotion, 0);
    }
}
