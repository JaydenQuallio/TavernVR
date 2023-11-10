using System.Collections.Generic;

public interface IGrogInterface
{
    void FillGrog(DrinkTypes drinkType, float fillAmount);
    float GetDrink(DrinkTypes drinks);
    List<float> GetAllDrinks();
    float GetFillAmount();
    float GetMaxAmount();
}
