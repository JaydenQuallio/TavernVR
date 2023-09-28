using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PatronInterface
{
    void SetPatronNumber(int pos);
    void SetLineNumber(int pos);
    void AdvanceLine();
}
