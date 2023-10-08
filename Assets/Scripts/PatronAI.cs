using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class PatronAI : MonoBehaviour, IPatronInterface
{

    [SerializeField]
    private Seeker seeker;
    [SerializeField]
    private Transform patronTrans;

    [SerializeField]
    private int patronNumber = 0, lineNumber = 0, nextPos = 0;

    [SerializeField]
    private float chanceMod, timeToSearch = 0f, maxTimeToSearch = 5f;

    private PatronManager patronManager;
    
    private bool isInPlace = false;

    private Dictionary<int, Transform> LinePoints = new();

    public enum LineState
    {
        None,
        Wandering,
        OrderWait,
        RecieveWait,
        ConsumingWait,
        Finished
    }

    [SerializeField]
    private LineState currentState;

    private void Awake()
    {
        patronManager = PatronManager.Instance;
    }

    private void Start()
    {
        currentState = LineState.None;
        chanceMod = patronManager.ChanceModifier;
    }

    private void OnEnable()
    {
        patronTrans = GetComponent<Transform>();
        patronManager.InitialAddToPatronList(this);
    }

    private void FixedUpdate()
    {
        if (currentState == LineState.None || currentState == LineState.Wandering)
        {
            GoInside();
        }
    }

    public void GoInside()
    {
        timeToSearch += Time.fixedDeltaTime;

        if (timeToSearch < maxTimeToSearch)
        {
            Wander();
        }
        else
        {
            if (patronManager.openPoints.Count > 0)
            {
                ChanceToEnter();
            }
        }
    }

    // Calculates the chance that the AI goes inside to order
    private void ChanceToEnter()
    {
        float temp = Random.Range(0f, 10f) * chanceMod;

        if (temp > 5f)
        {
            GetInLine();
        }
        else
        {
            timeToSearch = 0;
        }
    }

    [SerializeField]
    int postToMove = 0;
    [SerializeField]
    float timeToWait = 1f;

    private void Wander()
    {
        if (timeToWait < 0f)
        {
            if (!isInPlace && currentState == LineState.None)
            {
                postToMove = Random.Range(0, patronManager.roamingPoints.Count);
                MoveTo(patronManager.roamingPoints[postToMove].position);
                currentState = LineState.Wandering;
            }
            else if (currentState == LineState.Wandering && !isInPlace)
            {
                Debug.Log(Vector3.Distance(transform.position, patronManager.roamingPoints[postToMove].position) < 1f);
                if (Vector3.Distance(transform.position, patronManager.roamingPoints[postToMove].position) < 1f)
                {
                    isInPlace = true;
                    currentState = LineState.None;
                }
            }
            else
            {
                timeToWait = Random.Range(2f, 5f);
                isInPlace = false;
            }
        }
        else
            timeToWait -= Time.fixedDeltaTime;


    }

    // Sets the AI's number
    public void SetPatronNumber(int pos)
    {
        patronNumber = pos;
    }

    // Sets the AI's position in line
    private void SetLineNumber(int pos)
    {
        lineNumber = pos;
        nextPos = pos - 1;
    }

    // When called make the AI get in line and set their active state to waiting for their order
    private void GetInLine()
    {
        currentState = LineState.OrderWait;
        SetLineNumber(patronManager.openPoints[0]);
        patronManager.CloseSpot(patronManager.openPoints[0]);
        MoveTo(LinePoints[lineNumber].position);
    }

    // Method used to move AI the positions
    private void MoveTo(Vector3 spotToMove)
    {
        seeker.StartPath(patronTrans.position, spotToMove);
    }

    // When the AI is put into OrderWait State they will find a random spot to stand
    private void WaitForItem()
    {
        if (currentState == LineState.RecieveWait && seeker.IsDone())
        {
            MoveTo(patronManager.GenerateSpot());
        }
    }

    // Makes the AI move to the next spot
    // If their number is at the end of spots move the AI to waiting area
    public void AdvanceLine()
    {
        if (currentState != LineState.OrderWait)
        {
            return;
        }

        if (nextPos < 0)
        {
            patronManager.OpenSpot(lineNumber);
            currentState = LineState.RecieveWait;
            WaitForItem();
        }
        else
        {
            patronManager.CloseSpot(nextPos);
            patronManager.OpenSpot(lineNumber);
            SetLineNumber(nextPos);
            MoveTo(LinePoints[lineNumber].position);
        }
    }

    public void SetLineDictionary(Dictionary<int, Transform> dictionary) => LinePoints = dictionary;
    
}
