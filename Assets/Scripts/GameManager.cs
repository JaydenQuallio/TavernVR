using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [ShowInInspector]
    private float coin = 0f, baseTip = 5f;

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

    public void SetCoin(float coinToAdd)
    {
        coin += coinToAdd;
    }

    public float GetCoin() => coin;
    public float GetBaseCoin() => baseTip;
}
