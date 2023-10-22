using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/OrderScriptable")]
public class OrderScriptable : ScriptableObject
{
    public string drinkName;

    public DrinkModifiers modifer;

    public float red, green, blue;


    public float CompareDrink(float r, float g, float b)
    {
        float tempRed = Mathf.Abs(red - r);
        float tempGreen = Mathf.Abs(green - g);
        float tempBlue = Mathf.Abs(blue - b);

        float percent = 1f - (tempRed + tempGreen + tempBlue);

        Debug.Log(tempRed);
        Debug.Log(tempBlue);
        Debug.Log(tempGreen);
        Debug.Log(percent);

        return percent; // Returning how close they are as a percentage
    }
}
