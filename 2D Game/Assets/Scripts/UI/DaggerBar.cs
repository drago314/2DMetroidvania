using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaggerBar : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetDaggerCount(int currentDaggers)
    {
        image.fillAmount = currentDaggers / 3f;
    }
}
