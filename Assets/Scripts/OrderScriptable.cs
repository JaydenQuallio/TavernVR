using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/OrderScriptable")]
public class OrderScriptable : ScriptableObject
{
    public string drinkName;

    public DrinkModifiers modifer;

    public float red, green, blue;


    public float CompareDrink(Vector3 drinkValues)
    {
        float tempRed = Mathf.Abs(red - drinkValues.x);
        float tempGreen = Mathf.Abs(green - drinkValues.y);
        float tempBlue = Mathf.Abs(blue - drinkValues.z);

        float percent = 1f - (tempRed + tempGreen + tempBlue);

        Debug.Log(tempRed);
        Debug.Log(tempBlue);
        Debug.Log(tempGreen);
        Debug.Log(percent);

        return percent; // Returning how close they are as a percentage
    }
}
