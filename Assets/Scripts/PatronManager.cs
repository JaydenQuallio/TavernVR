using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    public List<Transform> openPoints = new();
    public List<Transform> takenPoints = new();

    [SerializeField]
    private List<IPatronInterface> Patrons = new();

    [SerializeField]
    private BoxCollider standArea;

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

    public void InitialAddToPatronList(IPatronInterface patron)
    {
        if (Patrons.Contains(patron))
            return;

        Patrons.Add(patron);
        patronCount++;
        Patrons[patronCount - 1].SetPatronNumber(patronCount);
        SetSpot(patronCount, patronCount - 1);
    }

    private void FixedUpdate()
    {
        if (progressLine)
        {
            progressLine = false;
            foreach(IPatronInterface patron in Patrons)
            {
                patron.AdvanceLine();
            }
        }
    }

    private void SetSpot(int patronID, int linePos)
    {
        Patrons[patronID - 1].SetLineNumber(linePos);
    }

    //Generates a random spot within the bounds provided
    public Vector3 GenerateSpot() => new (standArea.transform.position.x - Random.Range(-standArea.bounds.extents.x, standArea.bounds.extents.x), 0, standArea.transform.position.z - Random.Range(-standArea.bounds.extents.z, standArea.bounds.extents.z));
}
