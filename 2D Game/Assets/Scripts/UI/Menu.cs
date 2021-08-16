using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu : MonoBehaviour, IMenu
{
    private Canvas canvas;

    protected void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Close()
    {
        canvas.enabled = false;
    }

    public void Open()
    {
        canvas.enabled = true;
    }
}
