using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/OrderScriptable")]
public class OrderScriptable : ScriptableObject
{
    public string drinkName;

    public DrinkModifiers modifer;

    public float red, green, blue;


    public float CompareDrink(Vector3 drinkValues, float drinkAmount, float maxAmount)
    {
        float tempRed = red - drinkValues.x;
        float tempGreen = green - drinkValues.y;
        float tempBlue = blue - drinkValues.z;

        float percent = 1f;
        float fillDiff = drinkAmount / maxAmount;

        if(tempRed < 0)
        {
            percent += tempRed;
        }
        if(tempGreen < 0)
        {
            percent += tempGreen;
        }
        if (tempBlue < 0)
        {
            percent += tempBlue;
        }

        return (percent * fillDiff); // Returning how close they are as a percentage
    }
}
