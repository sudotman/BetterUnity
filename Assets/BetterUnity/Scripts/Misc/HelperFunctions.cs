using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public static HelperFunctions Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Scale a value from a previous range to a new range.
    /// </summary>
    /// <param name="OldMin">Original range's minimum value.</param>
    /// <param name="OldMax">Original range's maximum value.</param>
    /// <param name="NewMin">New range's minimum value.</param>
    /// <param name="NewMax">New range's maximum value.</param>
    /// <param name="OldValue">The value that is to be fit into the new range.</param>
    /// <returns>Returns a float scaled to the new specfied range.</returns>
    public float ScaleRange(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }




}
