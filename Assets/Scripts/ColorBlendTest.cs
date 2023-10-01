using UnityEngine;

public class ColorBlendTest : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(0, 1)]
    public float integerRange;

    public Material redColor, blueColor, tempColor;

    void Start()
    {
        tempColor = new Material(redColor);
        GetComponent<Renderer>().material = tempColor;
    }

    private void OnDisable()
    {
        Destroy(tempColor);
    }

    private void Update()
    {
        tempColor.SetColor("_TopColor", Color.Lerp(redColor.GetColor("_TopColor"), blueColor.GetColor("_TopColor"), integerRange));
        tempColor.SetColor("_BottomColor", Color.Lerp(redColor.GetColor("_BottomColor"), blueColor.GetColor("_BottomColor"), integerRange));
        tempColor.SetColor("_FoamColor", Color.Lerp(redColor.GetColor("_FoamColor"), blueColor.GetColor("_FoamColor"), integerRange));
        tempColor.SetColor("_Rim_Color", Color.Lerp(redColor.GetColor("_Rim_Color"), blueColor.GetColor("_BottomColor"), integerRange));
    }
}
