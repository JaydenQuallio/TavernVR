using UnityEngine;
using Pathfinding;

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

    [SerializeField]
    private PlayerStates currentState;

    [SerializeField]
    private GameObject patronOrder;
    private IOrderInterface orderInterface;

    private void Awake()
    {
        patronManager = PatronManager.Instance;
    }

    private void Start()
    {
        currentState = PlayerStates.None;
        chanceMod = patronManager.ChanceModifier;
        orderInterface = patronOrder.GetComponent<IOrderInterface>();

        patronOrder.transform.parent = patronManager.GetOrderTrans();
        patronOrder.transform.position = patronManager.GetOrderTrans().position;
        patronOrder.transform.rotation = patronManager.GetOrderTrans().rotation;
    }

    private void OnEnable()
    {
        patronTrans = GetComponent<Transform>();
        patronManager.InitialAddToPatronList(this);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerStates.None || currentState == PlayerStates.Wandering)
        {
            timeToSearch += Time.fixedDeltaTime;
            GoInside();
        }
    }

    public void GoInside()
    {
        if (timeToSearch < maxTimeToSearch)
        {
            Wander();
        }
        else
        {
            if (Random.Range(0f, 10f) * chanceMod > 5f)
            {
                AddToList();
            }
            else
            {
                timeToSearch = 0;
            }
        }
    }

    [SerializeField]
    int postToMove = -1;
    [SerializeField]
    float timeToWait = 1f;

    private void Wander()
    {
        if (timeToWait < 0f)
        {
            if (!isInPlace && currentState == PlayerStates.None)
            {
                ChooseRandomRoamingPoint();
            }
            else if (currentState == PlayerStates.Wandering && !isInPlace)
            {
                CheckIfInPlace();
            }
            else
            {
                SetRandomWaitTime();
            }
        }
        else
        {
            timeToWait -= Time.fixedDeltaTime;
        }
    }

    private void ChooseRandomRoamingPoint()
    {
        postToMove = Random.Range(0, patronManager.GetRoamingPoints());
        MoveTo(patronManager.RoamingPointIndex(postToMove).position);
        currentState = PlayerStates.Wandering;
    }

    private void CheckIfInPlace()
    {
        if (Vector3.Distance(transform.position, patronManager.RoamingPointIndex(postToMove).position) < 1f)
        {
            isInPlace = true;
            currentState = PlayerStates.None;
        }
    }

    private void SetRandomWaitTime()
    {
        timeToWait = Random.Range(2f, 5f);
        isInPlace = false;
    }

    // Sets the AI's number
    public void SetPatronNumber(int pos)
    {
        patronNumber = pos;
    }

    public int GetPatronNumber() => patronNumber;

    // When called make the AI get in line and set their active state to waiting for their order
    private void AddToList() => patronManager.AddPlayerToLine(this);

    // Method used to move AI the positions
    public void MoveTo(Vector3 spotToMove) => seeker.StartPath(patronTrans.position, spotToMove);

    public void SetOrderState(PlayerStates state) => currentState = state;
    public PlayerStates GetOrderState() => currentState;

    // When the AI is put into OrderWait State they will find a random spot to stand
    public void WaitForItem()
    {
        if (currentState == PlayerStates.OrderWait && seeker.IsDone() && nextPos < 0)
        {
            SetOrderState(PlayerStates.RecieveWait);
            MoveTo(patronManager.GenerateSpot());
        }
    }

    public void SetLineNumber(int pos)
    {
        lineNumber = pos;
        nextPos = pos - 1;

        if(nextPos < 0)
        {
            patronOrder.SetActive(true);
            orderInterface.SetOrder(patronNumber, orderInterface.GenerateOrder());
        }
    }

    public int GetLineNumber() => lineNumber;
    public int GetNextPos() => nextPos;
}
