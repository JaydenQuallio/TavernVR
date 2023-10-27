using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PatronManager : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<int, Transform> LinePoints = new();

    // Points to walk to
    private List<int> walkingPoints = new();

    [SerializeField]
    private List<Transform> roamingPoints = new();

    [SerializeField]
    private GameObject orderPref;

    [SerializeField]
    private Transform noteTrans;

    //Interfaces
    private List<IPatronInterface> Patrons = new();
    private List<IPatronInterface> QueuedInLine = new();
    private List<IPatronInterface> playersInLine = new();

    [SerializeField]
    private BoxCollider standArea;

    private int patronCount = 0;

    private float chanceToEnter = 1.33f;

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

        foreach (KeyValuePair<int, Transform> kvp in LinePoints)
        {
            walkingPoints.Add(kvp.Key);
        }
    }

    private void FixedUpdate()
    {
        if (walkingPoints.Count > 0)
        {
            foreach (int point in walkingPoints.ToArray())
            {
                if (QueuedInLine.Count <= 0)
                    continue;

                QueuedInLine[0].SetOrderState(PlayerStates.OrderWait);

                patronCount++;
                QueuedInLine[0].SetPatronNumber(patronCount);

                QueuedInLine[0].SetLineNumber(point);
                QueuedInLine[0].MoveTo(LinePoints[QueuedInLine[0].GetLineNumber()].position);

                CloseSpot(0);
                walkingPoints.Remove(point);
            }
        }
    }

    private void CloseSpot(int spot)
    {
        playersInLine.Add(QueuedInLine[spot]);
        QueuedInLine.Remove(QueuedInLine[spot]);
    }

    private void OpenSpots()
    {
        int open = LinePoints.Count - playersInLine.Count;

        for (int i = 0; i < open; i++)
        {
            if (!walkingPoints.Contains((LinePoints.Count - 1) - i))
            {
                walkingPoints.Add((LinePoints.Count - 1) - i);
                walkingPoints = walkingPoints.OrderBy(num => num).ToList();
            }
        }
    }

    public void InitialAddToPatronList(IPatronInterface patron)
    {
        if (Patrons.Contains(patron))
            return;

        Patrons.Add(patron);
    }

    public void AddPlayerToLine(IPatronInterface player)
    {
        if (!QueuedInLine.Contains(player))
            QueuedInLine.Add(player);
    }

    // Method used to progress the line when needed
    [Button]
    public void ProgressLine()
    {
        if (playersInLine.Count < 1)
            return;

        playersInLine[0].WaitForItem();
        playersInLine.RemoveAt(0);

        foreach (IPatronInterface player in playersInLine)
        {
            player.SetLineNumber(player.GetNextPos());
            player.MoveTo(LinePoints[player.GetLineNumber()].position);
        }

        OpenSpots();
    }

    public int GetRoamingPoints() => roamingPoints.Count;
    public Transform RoamingPointIndex(int point) => roamingPoints[point];

    // Generates a random spot within the bounds provided
    public Vector3 GenerateSpot() => new(standArea.transform.position.x - Random.Range(-standArea.bounds.extents.x, standArea.bounds.extents.x), 0, standArea.transform.position.z - Random.Range(-standArea.bounds.extents.z, standArea.bounds.extents.z));

    public float ChanceModifier { get { return chanceToEnter; } set { chanceToEnter = value; } }

    public Transform GetOrderTrans() => noteTrans;
}
