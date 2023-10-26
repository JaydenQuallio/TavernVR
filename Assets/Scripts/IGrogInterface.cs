using UnityEngine;

public interface IGrogInterface
{
    void FillGrog(DrinkTypes drinkType, float fillAmount);
    Vector3 GetDrink();
    float GetFillAmount();
    float GetMaxAmount();
}
