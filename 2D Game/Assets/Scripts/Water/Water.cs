using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Liquid
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            collision.GetComponent<IWet>().OnEnterLiquid(this);
        }
        catch(System.Exception) {}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            collision.GetComponent<IWet>().OnExitLiquid(this);
        }
        catch (System.Exception) { }
    }
}
