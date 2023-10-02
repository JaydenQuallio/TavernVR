using System.Collections.Generic;
using UnityEngine;

public class GrogScript : MonoBehaviour, IGrogInterface
{
    private Dictionary<DrinkTypes, float> drinkList = new();
    private Dictionary<DrinkTypes, Material> drinkTypeList = new();

    [SerializeField]
    private LiquidScript liquidScript;

    [SerializeField]
    private float totalLiquid;

    [SerializeField]
    private Material bluePotion, redPotion, greenPotion, tempMat = null;

    [SerializeField]
    private Renderer liquidRenderer;

    [SerializeField]
    private ParticleSystem foam;

    private bool hasIntialColor = false;

    private void Start()
    {
        liquidScript.enabled = false;
    }

    private void OnEnable()
    {
        SetPotionStats();

        GrogManager.Instance.AddGrogToList(this, gameObject);
    }

    private void OnDisable()
    {
        liquidScript.enabled = false;
    }

    private void FixedUpdate()
    {
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);

        if (tiltAngle > 90f)
        {
            Debug.Log("Object tilted more than 90 degrs!");
        }
    }

    public void FillGrog(DrinkTypes drinkType, float fillAmount)
    {
        if(liquidScript.enabled == false)
            liquidScript.enabled = true;

        if(!foam.isPlaying)
            foam.Play();

        totalLiquid += fillAmount;
        drinkList[drinkType] += fillAmount;
        IncreaseLiquid(totalLiquid);
    }


    public void StopGrog()
    {
        if (foam.isPlaying)
            foam.Stop();
    }

    private void IncreaseLiquid(float liquidAmount)
    {
        liquidScript.SetAmount = Mathf.Lerp(.556f, .45f, liquidAmount);
        ChangeDrinkColor();

    }

    private void ChangeDrinkColor()
    {
        if(!hasIntialColor)
        {
            foreach(KeyValuePair<DrinkTypes, float> drink in drinkList)
            {
                if (drink.Value > 0)
                {
                    if (drinkTypeList.ContainsKey(drink.Key))
                    {
                        SetColor(drinkTypeList[drink.Key]);
                        hasIntialColor = true;
                    }
                }
            }
        }

        ColorToLerp(bluePotion, drinkList[DrinkTypes.BluePotion]);
        ColorToLerp(redPotion, drinkList[DrinkTypes.RedPotion]);
        ColorToLerp(greenPotion, drinkList[DrinkTypes.BluePotion]);
    }

    private void ColorToLerp(Material matToChange, float liquidAmount)
    {
        tempMat.SetColor("_TopColor", Color.Lerp(tempMat.GetColor("_TopColor"), matToChange.GetColor("_TopColor"), liquidAmount));
        tempMat.SetColor("_BottomColor", Color.Lerp(tempMat.GetColor("_BottomColor"), matToChange.GetColor("_BottomColor"), liquidAmount));
        tempMat.SetColor("_FoamColor", Color.Lerp(tempMat.GetColor("_FoamColor"), matToChange.GetColor("_FoamColor"), liquidAmount));
        tempMat.SetColor("_Rim_Color", Color.Lerp(tempMat.GetColor("_Rim_Color"), matToChange.GetColor("_BottomColor"), liquidAmount));
    }

    private void SetColor(Material mat)
    {
        tempMat = new Material(mat);
        liquidRenderer.material = tempMat;
    }

    void SetPotionStats()
    {
        drinkList.Clear();
        drinkTypeList.Clear();

        drinkTypeList.Add(DrinkTypes.RedPotion, redPotion);
        drinkTypeList.Add(DrinkTypes.BluePotion, bluePotion);
        drinkTypeList.Add(DrinkTypes.GreenPotion, greenPotion);

        foreach (KeyValuePair<DrinkTypes, Material> drinkType in drinkTypeList)
        {
            if (drinkList.ContainsKey(drinkType.Key))
            {
                return;
            }
            drinkList.Add(drinkType.Key, 0);
        }
    }

}
