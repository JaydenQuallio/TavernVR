using UnityEngine;
using Pathfinding;

public class PatronAI : MonoBehaviour, PatronInterface
{

    [SerializeField]
    private Seeker seeker;
    [SerializeField]
    private Transform patronTrans;

    [SerializeField]
    private int patronNumber = 0, lineNumber = 0;

    public enum LineState
    {
        None,
        OrderWait,
        RecieveWait,
        ConsumingWait,
        Finished
    }

    [SerializeField]
    private LineState currentState;

    private void Start()
    {
        currentState = LineState.None;
    }

    private void OnEnable()
    {
        patronTrans = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (currentState == LineState.None)
        {
            GetInLine();
        }
    }

    public void SetPatronNumber(int pos)
    {
        patronNumber = pos;
    }

    public void SetLineNumber(int pos)
    {
        lineNumber = pos;
    }

    private void GetInLine()
    {
        if (seeker.IsDone())
        {
            currentState = LineState.OrderWait;
            seeker.StartPath(patronTrans.position, PatronManager.Instance.openPoints[lineNumber].position);
        }
    }

    private void WaitForItem()
    {
        if (seeker.IsDone() && currentState == LineState.OrderWait)
        {
            currentState = LineState.RecieveWait;
            seeker.StartPath(patronTrans.position, PatronManager.Instance.openPoints[lineNumber].position);
        }
    }

    public void AdvanceLine()
    {
        if (lineNumber < 1)
            WaitForItem();
        else
            lineNumber--;
    }

}
