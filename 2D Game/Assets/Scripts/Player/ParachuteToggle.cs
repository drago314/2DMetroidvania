using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParachuteToggle : MonoBehaviour
{

    public void Open()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Close()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}
