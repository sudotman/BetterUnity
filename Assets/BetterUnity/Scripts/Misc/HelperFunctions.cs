using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{

    //Instead of an instance and calling them through this, using an extension class is much better.


    /* Archived Code
    //public static HelperFunctions Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(this);
    //        return;
    //    }
    //    Instance = this;
    //}

    */


    /// <summary>
    /// Scale a value from a previous range to a new range (remap it linearly).
    /// </summary>
    /// <param name="OldMin">Original range's minimum value.</param>
    /// <param name="OldMax">Original range's maximum value.</param>
    /// <param name="NewMin">New range's minimum value.</param>
    /// <param name="NewMax">New range's maximum value.</param>
    /// <param name="OldValue">The value that is to be fit into the new range.</param>
    /// <returns>A float scaled to the new specfied range.</returns>
    public static float ScaleRange(this float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    /// <summary>
    /// Destroy all children of a parent.
    /// </summary>
    /// <param name="_transform">Parent transform.</param>
    public static void DestroyChildren(this Transform _transform)
    {
        foreach (Transform child in _transform)
        {
            Object.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Subtract one float value from x, y, z components of a Vector3.
    /// </summary>
    /// <param name="vector">Vector to be subtracted from..</param>
    /// <param name="f">The value to be subtracted from the Vector3.</param>
    /// <returns>A new Vector3 with subtracted value.</returns>
    public static Vector3 MinusFloatVector3(this Vector3 vector, float f)
    {
        return new Vector3(vector.x - f, vector.y - f, vector.z - f);
    }

    // Interpolations

    /// <summary>
    /// Returns an Ease-Out interpolation between two Vector3, the float it is called from is the t time.
    /// </summary>
    /// <param name="start">The start value in Vector3.</param>
    /// <param name="end">The end value in Vector3.</param>
    /// <returns>A Vector3 with the required interpolation.</returns>
    public static Vector3 EaseOutLerp(this Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(start, end, t);
    }

    /// <summary>
    /// Returns an Ease-In interpolation between two Vector3, the float it is called from is the t time.
    /// </summary>
    /// <param name="start">The start value in Vector3.</param>
    /// <param name="end">The end value in Vector3.</param>
    /// <returns>A Vector3 with the required interpolation.</returns>
    public static Vector3 EaseInLerp(this Vector3 start, Vector3 end, float t)
    {
        t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        return Vector3.Lerp(start, end, t);
    }

    /// <summary>
    /// Returns a Smooth Stepping interpolation between two floats.
    /// </summary>
    /// <param name="start">The start value in float.</param>
    /// <param name="end">The end value in float.</param>
    /// <returns>A float with the required interpolation.</returns>
    public static float Smoothstep(float t,float start, float end)
    {
        t = t * t * (3f - 2f * t);
        return Mathf.Lerp(start, end, t);
    }



    //Transform based stuff


    /// <summary>
    /// Look at but rotate only across the Y axis (say, for an enemy that needs to turn to you at all times).
    /// </summary>
    /// <param name="point">The point to look at.</param>
    public static void LookAtY(this Transform transform, Vector3 point)
    {
        var lookPos = point - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }


    /// <summary>
    /// Give back a grounded Vector3 (ignoring the Y-position).
    /// </summary>
    /// <returns>A grounded Vector3.</returns>
    public static Vector3 Grounded(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    /// <summary>
    /// Returns distance between two vectors but its grounded. (ignoring their Y-position during calculation).
    /// <param name="destination">The destination vector.</param>
    /// </summary>
    /// <returns>A float with the calculated distance.</returns>
    public static float DistanceFromGround(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.Grounded(), destination.Grounded());
    }


    /// <summary>
    /// Get a random item from a list.
    /// </summary>
    /// <returns>A random item.</returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[Random.Range(0, list.Count)];
    }

    //06 - July
    /// <summary>
    /// Changes a Vector3 without modifying the Y-component.
    /// <param name="keepYFrom">The Vector3 from which we want the Y-Position to retain.</param>
    /// </summary>
    /// <returns>A Vector3 with the calculated distance.</returns>
    public static Vector3 ExcludeY(this Vector3 currentVec, Vector3 keepYFrom)
    {
        return new Vector3(currentVec.x,keepYFrom.y,currentVec.z);
    }



}
