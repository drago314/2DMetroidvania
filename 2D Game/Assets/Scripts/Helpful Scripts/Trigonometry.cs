using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigonometry 
{
    public static float DegreeToRadian(float degree)
    {
        return degree * Mathf.PI / 180;
    }

    public static float RadianToDegree(float radian)
    {
        return radian * 180 / Mathf.PI;
    }

    public static float RadianFromPosition(float x, float y)
    {
        float radian = Mathf.Atan(Mathf.Abs(y / x));
        return CheckRadian(x, y, radian);
    }

    public static float RadianFromPosition(Vector2 position)
    {
        return RadianFromPosition(position.x, position.y);
    }

    private static float CheckRadian(float x, float y, float radianPrime)
    {
        if (y < 0 && x < 0)
            return radianPrime + Mathf.PI;
        else if (y < 0 && x != 0)
            return 2 * Mathf.PI - radianPrime;
        else if (x < 0 && y != 0)
            return Mathf.PI - radianPrime;
        else if (x > 0 && y > 0)
            return radianPrime;
        else if (y == 0 && x > 0)
            return 0;
        else if (y == 0 && x < 0)
            return Mathf.PI;
        else if (y > 0)
            return Mathf.PI / 2f;
        else if (y < 0)
            return 3 * Mathf.PI / 2f;
        else
            return 0;
    }
}
