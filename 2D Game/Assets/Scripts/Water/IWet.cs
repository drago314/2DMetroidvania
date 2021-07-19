using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWet
{
    void OnEnterLiquid(Liquid liquid);
    void OnExitLiquid(Liquid liquid);
}
