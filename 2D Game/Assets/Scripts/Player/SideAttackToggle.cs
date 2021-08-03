using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttackToggle : MonoBehaviour
{
    private SpriteRenderer sp;
    private float position;

    private void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
        position = transform.localPosition.x;
    }

    public void Attack(bool facing)
    {
        if (facing)
        {
            transform.localPosition = new Vector3(position, transform.localPosition.y, transform.localPosition.z);
            sp.flipX = false;
            
        }
        else
        {
            transform.localPosition = new Vector3(-position, transform.localPosition.y, transform.localPosition.z);
            sp.flipX = true;
        }
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void EndAttack()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
