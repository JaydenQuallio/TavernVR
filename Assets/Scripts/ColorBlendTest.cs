using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlendTest : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(0, 1)]
    public int integerRange;

    public Material color, tempColor;

    void Start()
    {
        tempColor = new Material(color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
