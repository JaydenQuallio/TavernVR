using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    public List<Transform> openPoints = new();
    public List<Transform> takenPoints = new();
    [SerializeField]
    private List<PatronInterface> Patrons = new();

    int patronCount = 0;

    [SerializeField]
    bool progressLine;

    public static PatronManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is PatronInterface)
            {
                Patrons.Add(allScripts[i] as PatronInterface);
                patronCount++;
                Patrons[patronCount - 1].SetPatronNumber(patronCount);
                SetSpot(patronCount, patronCount - 1);
            }
        }
    }

    private void FixedUpdate()
    {
        if (progressLine)
        {
            progressLine = false;
            foreach(PatronInterface patron in Patrons)
            {
                patron.AdvanceLine();
            }
        }
    }

    private void SetSpot(int patronID, int linePos)
    {
        Patrons[patronID - 1].SetLineNumber(linePos);
    }

}
