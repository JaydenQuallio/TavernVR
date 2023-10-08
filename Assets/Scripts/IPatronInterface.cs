using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatronInterface
{
    void SetPatronNumber(int pos);
    void AdvanceLine();
    void SetLineDictionary(Dictionary<int, Transform> dictionary);
}
