using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrderInterface 
{
    void SetOrder(int orderNum, IPatronInterface patron); 
}
