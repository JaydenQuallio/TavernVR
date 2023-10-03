using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
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
        
        //Amount of liquid that is filled
        liquidScript.SetAmount = Mathf.Lerp(.6f, .45f, totalLiquid);
        ChangeDrinkColor(drinkType, fillAmount);
    }


    public void StopGrog()
    {
        if (foam.isPlaying)
            foam.Stop();
    }

    private void ChangeDrinkColor(DrinkTypes type, float fillAmount)
    {
        if (!hasIntialColor)
        {
            SetColor(drinkTypeList[type]);
            hasIntialColor = true;
        }
        
        drinkList[type] += fillAmount;

        ColorToLerp(drinkTypeList[type], drinkList[type] / totalLiquid);
    }

    private void ColorToLerp(Material matToChange, float liquidAmount)
    {
        liquidAmount *= Time.fixedDeltaTime;

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
