using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatronInterface
{
    void SetOrderState(PlayerStates state);
    PlayerStates GetOrderState();
    void MoveTo(Vector3 spotToMove);
    void SetPatronNumber(int pos);
    int GetPatronNumber();
    void SetLineNumber(int pos);
    int GetLineNumber();
    void WaitForItem();
    int GetNextPos();
}
