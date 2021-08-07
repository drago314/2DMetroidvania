using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToggle : MonoBehaviour
{
    public void Attack()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void EndAttack()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
