using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/OrderScriptable")]
public class OrderScriptable : ScriptableObject
{
    public string drinkName;

    public DrinkModifiers modifer;

    public float red, green, blue, white, black;

    private float finRed, finGreen, finBlue, finWhite, finBlack;

    public void CalculateDrink(float tred, float tgreen, float tblue, float twhite, float tblack)
    {
        finRed = red - tred;
        finGreen = green - tgreen;
        finBlue = blue - tblue;
        finWhite = white - twhite;
        finBlack = black - tblack;
    }

    public float CompareDrink(float drinkAmount, float maxAmount)
    {
        float percent = 1f;
        float fillDiff = drinkAmount / maxAmount;

        if(finRed < 0)
        {
            percent += finRed;
        }
        if(finGreen < 0)
        {
            percent += finGreen;
        }
        if (finBlue < 0)
        {
            percent += finBlue;
        }
        if (finWhite < 0)
        {
            percent += finWhite;
        }
        if (finBlack < 0)
        {
            percent += finBlack;
        }

        return (percent * fillDiff); // Returning how close they are as a percentage
    }
}
