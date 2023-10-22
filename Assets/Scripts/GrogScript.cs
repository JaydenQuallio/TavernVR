using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GrogScript : SerializedMonoBehaviour, IGrogInterface
{
    [ShowInInspector]
    private Dictionary<DrinkTypes, float> drinkList = new();

    [SerializeField]
    private LiquidScript liquidScript;

    [SerializeField]
    private float totalLiquid;

    [SerializeField]
    private Renderer liquidRenderer;

    [SerializeField]
    private ContainerTypes container;

    private bool hasIntialColor = false;

    private Material tempMat = null;
    private References references;

    private void Awake()
    {
        references = References.instance;
        liquidScript.enabled = false;
    }

    private void OnEnable()
    {
        SetPotionStats();

        GrogManager.instance.AddGrogToList(this, gameObject);
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

        totalLiquid += fillAmount;
        
        //Amount of liquid that is filled
        liquidScript.SetAmount = Mathf.Lerp(.6f, .45f, totalLiquid);
        ChangeDrinkColor(drinkType, fillAmount);
    }
     
    private void ChangeDrinkColor(DrinkTypes type, float fillAmount)
    {
        if (!hasIntialColor)
        {
            SetColor(references.GetDrinkMaterial(type));
            hasIntialColor = true;
        }
        
        drinkList[type] += fillAmount;

        ColorToLerp(references.GetDrinkMaterial(type), drinkList[type] / totalLiquid);
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

        foreach (KeyValuePair<DrinkTypes, Material> drinkType in references.DrinkTypeList)
        {
            if (drinkList.ContainsKey(drinkType.Key))
            {
                return;
            }
            drinkList.Add(drinkType.Key, 0);
        }
    }

    public float GetDrinkType()
    {
        foreach(KeyValuePair<DrinkTypes, float> kp in drinkList)
        {
            return kp.Value;
        }
        return 0;
    }

    public Vector3 GetDrink()
    {
        return Vector3.zero;
    }
}
