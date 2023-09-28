using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatronData", menuName = "ScriptableObjects/PatronStats", order = 1)]
public class PatronScriptable : ScriptableObject
{
    public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

}
