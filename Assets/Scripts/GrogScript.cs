using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GrogScript : SerializedMonoBehaviour, IGrogInterface
{
    private Dictionary<DrinkTypes, float> drinkList = new();

    [SerializeField]
    private LiquidScript liquidScript;

    [SerializeField]
    private float totalLiquid, maxLiquid = 1f, emptyAmount = .075f;

    [SerializeField]
    private float maxLiquidMesh = .625f, minLiquidMesh = .45f;

    [SerializeField]
    private Renderer liquidRenderer;

    [SerializeField]
    private ContainerTypes container;

    private bool hasIntialColor = false, hasBeenTouched = false;

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

        GrogManager.Instance.AddGrogToList(this, gameObject);
    }

    private void OnDisable()
    {
        liquidScript.enabled = false;
    }

    private void FixedUpdate()
    {
        float tiltAngle = Vector3.Angle(Vector3.up, transform.up);

        if (tiltAngle > 90f && totalLiquid > 0f)
        {
            EmptyGrog(emptyAmount * Time.fixedDeltaTime);
            Debug.Log("Object tilted more than 90 degrs!");
        }
    }

    public void FillGrog(DrinkTypes drinkType, float fillAmount)
    {
        if (liquidScript.enabled == false)
            liquidScript.enabled = true;

        if (totalLiquid >= maxLiquid)
            EmptyGrog(fillAmount); // Pour out the excess
        else
        {
            totalLiquid += fillAmount;
            drinkList[drinkType] += fillAmount;
        }

        //Amount of liquid that is filled 
        SetLiquidHeight(maxLiquidMesh, minLiquidMesh, totalLiquid);
        ChangeDrinkColor(drinkType, fillAmount);
    }

    private void EmptyGrog(float empty)
    {
        for (int i = 0; i < drinkList.Count - 1; i++)
        {
            if (drinkList[(DrinkTypes)i] > 0f)
            {
                totalLiquid -= empty;
                drinkList[(DrinkTypes)i] -= empty;
            }
            if (drinkList[(DrinkTypes)i] <= 0f)
            {
                drinkList[(DrinkTypes)i] = 0f;
            }
        }
        if (totalLiquid < 0f)
            totalLiquid = 0f;

        SetLiquidHeight(maxLiquidMesh, minLiquidMesh, totalLiquid);
    }

    private void ChangeDrinkColor(DrinkTypes type, float fillAmount)
    {
        if (!hasIntialColor)
        {
            SetColor(references.GetDrinkMaterial(type));
            hasIntialColor = true;
        }

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

    public void OnPickUp()
    {
        if (!hasBeenTouched)
        {
            GrogManager.Instance.SpawnNextGrog();
            hasBeenTouched = true;
        }
    }

    private void SetPotionStats()
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

    private void SetLiquidHeight(float maxHeight, float minHeight, float total) => liquidScript.SetAmount = Mathf.Lerp(maxHeight, minHeight, total);

    public float GetFillAmount() => totalLiquid;
    public float GetMaxAmount() => maxLiquid;

    public float GetDrink(DrinkTypes type) => drinkList[type];

    public List<float> GetAllDrinks() => new() { GetDrink(DrinkTypes.RedPotion), GetDrink(DrinkTypes.GreenPotion), GetDrink(DrinkTypes.BluePotion), GetDrink(DrinkTypes.WhitePotion), GetDrink(DrinkTypes.BlackPotion)};
}
